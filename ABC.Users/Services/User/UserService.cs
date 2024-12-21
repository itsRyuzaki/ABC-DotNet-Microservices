using ABC.Users.DTO;
using ABC.Users.Enums;
using ABC.Users.Models;
using AutoMapper;
using MongoDB.Driver;

namespace ABC.Users.Services;

public class UserService(IMongoDBService mongoService, IMapper _mapper, ILogger<UserService> _logger) : IUserService
{
    private readonly IMongoCollection<User> _userCollection = mongoService.GetUserCollection();
    private readonly IMongoCollection<UserAuth> _userAuthCollection = mongoService.GetUserAuthCollection();

    public async Task<ApiResponseDto> AddUserAsync(UserSignUpDto signUpRequest)
    {
        var userData = _mapper.Map<User>(signUpRequest);

        var authData = new UserAuth()
        {
            UserName = signUpRequest.UserName,
            FirstName = signUpRequest.FirstName,
            AuthHash = signUpRequest.Password,
            AccessibleModules = ["Employee"],
            Active = true
        };

        try
        {
            await _userCollection.InsertOneAsync(userData);
        }
        catch (MongoWriteException error)
        {
            _logger.LogError("User already exists in User Collection");

            if (error.WriteError.Code == (int)ResponseCode.DUPLICATE)
            {
                return GetUserExistsError(error.WriteError.Code, ["User already exists in User Collection"]);

            }

            throw;

        }
        catch (Exception error)
        {
            _logger.LogError("Error while saving user details, error stack below: \n {ERROR} ", error.ToString());
            return GetUserExistsError((int)ResponseCode.ERROR, ["Error while saving user data"]);

        }


        try
        {
            await _userAuthCollection.InsertOneAsync(authData);
        }
        catch (MongoWriteException error)
        {
            _logger.LogError("User already exists in UserAuth Collection");

            if (error.WriteError.Code == (int)ResponseCode.DUPLICATE)
            {
                _logger.LogInformation("Deleting saved user data from User Collection");
                await _userCollection.DeleteOneAsync(data => data.UserName == userData.UserName);
                _logger.LogInformation("Successfully deleted saved user data from User Collection");
                
                return GetUserExistsError(error.WriteError.Code, ["User already exists in UserAuth Collection"]);
            }

            throw;

        }
        catch (Exception error)
        {
            _logger.LogError("Error while saving user details, error stack below: \n {ERROR} ", error.ToString());
            await _userCollection.DeleteOneAsync(data => data.UserName == userData.UserName);

            return GetUserExistsError((int)ResponseCode.ERROR, ["Error while saving user data"]);

        }

        return new ApiResponseDto()
        {
            Success = true,
        };

    }

    private ApiResponseDto GetUserExistsError(int code, string[] details)
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