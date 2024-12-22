using ABC.Users.DTO;
using ABC.Users.Enums;
using ABC.Users.Models;
using AutoMapper;
using MongoDB.Driver;

namespace ABC.Users.Services;

public class UserService(IMongoDBService mongoService, ILogger<UserService> _logger) : IUserService
{
    private readonly IMongoCollection<User> _userCollection = mongoService.GetUserCollection();
    private readonly IMongoCollection<UserAuth> _userAuthCollection = mongoService.GetUserAuthCollection();

    // public async Task LoginUserAsync(UserLoginDto loginRequest){}

    public async Task<ApiResponseDto> InsertUserDataAsync(User userData)
    {
        try
        {
            await _userCollection.InsertOneAsync(userData);
        }
        catch (MongoWriteException error)
        {
            _logger.LogError("User already exists in User Collection");

            if (error.WriteError.Code == (int)ResponseCode.DUPLICATE)
            {
                return HandleErrorResponse(error.WriteError.Code, ["User already exists in User Collection"]);
            }

            throw;

        }
        catch (Exception error)
        {
            _logger.LogError("Error while saving user details, error stack below: \n {ERROR} ", error.ToString());
            return HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving user data"]);

        }

        return new ApiResponseDto()
        {
            Success = true,
        };
    }

    public async Task<ApiResponseDto> InsertUserAuthDataAsync(UserAuth authData)
    {
        try
        {
            await _userAuthCollection.InsertOneAsync(authData);
        }
        catch (MongoWriteException error)
        {
            _logger.LogError("User already exists in UserAuth Collection");

            if (error.WriteError.Code == (int)ResponseCode.DUPLICATE)
            {
                // Delete user data which is added to maintain consistency
                await DeleteUserData(authData.UserName);

                return HandleErrorResponse(error.WriteError.Code, ["User already exists in UserAuth Collection"]);
            }

            throw;

        }
        catch (Exception error)
        {
            _logger.LogError("Error while saving user details, error stack below: \n {ERROR} ", error.ToString());

            // Delete user data which is added to maintain consistency
            await DeleteUserData(authData.UserName);

            return HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving user data"]);

        }

        return new ApiResponseDto()
        {
            Success = true,
        };
    }

    public async Task<UserAuth> GetUserAuthAsync(string userName){
       var result = await _userAuthCollection.FindAsync(auth => auth.UserName == userName);
        return result.FirstOrDefault();
    }

    private async Task DeleteUserData(string UserName)
    {
        _logger.LogInformation("Deleting saved user data from User Collection");
        await _userCollection.DeleteOneAsync(data => data.UserName == UserName);
        _logger.LogInformation("Successfully deleted saved user data from User Collection");
    }

    private ApiResponseDto HandleErrorResponse(int code, string[] details)
    {
        return new ApiResponseDto()
        {
            Success = false,
            ErrorDetails = new ErrorDetails()
            {
                Code = code,
                Details = details
            }
        };
    }
}