using ABC.Users.DTO.Response;
using ABC.Users.Enums;
using ABC.Users.Models;
using AutoMapper;
using MongoDB.Driver;

namespace ABC.Users.Services;

public class UserService
                (
                    IMongoDBService mongoService,
                    ILogger<UserService> _logger
                ) : IUserService
{
    private readonly IMongoCollection<User> _userCollection = mongoService.GetUserCollection();
    private readonly IMongoCollection<UserAuth> _userAuthCollection = mongoService.GetUserAuthCollection();

    private readonly IMongoCollection<SessionHistory> _sessionCollection = mongoService.GetSessionHistoryCollection();


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
                return ApiResponseDto.HandleErrorResponse(error.WriteError.Code, ["User already exists in User Collection"]);
            }

            throw;

        }
        catch (Exception error)
        {
            _logger.LogError("Error while saving user details, error stack below: \n {ERROR} ", error.ToString());
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving user data"]);

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

                return ApiResponseDto.HandleErrorResponse(error.WriteError.Code, ["User already exists in UserAuth Collection"]);
            }

            throw;

        }
        catch (Exception error)
        {
            _logger.LogError("Error while saving user details, error stack below: \n {ERROR} ", error.ToString());

            // Delete user data which is added to maintain consistency
            await DeleteUserData(authData.UserName);

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving user data"]);

        }

        return new ApiResponseDto()
        {
            Success = true,
        };
    }

    public async Task<UserAuth?> GetUserAuthAsync(string userName)
    {
        _logger.LogInformation("Fetching userAuth data for username: {username}", userName);

        try
        {
            var result = await _userAuthCollection.FindAsync(auth => auth.UserName == userName);
            _logger.LogInformation("Succesfully fetched userAuth data for username: {username}", userName);


            return result.FirstOrDefault();

        }
        catch (Exception error)
        {
            _logger.LogError(
                    "Error while fetching userAuth data for username: {username}, see error stack below:\n {error}",
                    userName,
                    error
            );

            return null;
        }
    }

    public async Task<string?> GetAndSetSessionToken(string userName)
    {
        string sessionToken = Guid.NewGuid().ToString();

        var filter = Builders<SessionHistory>.Filter;
        var update = Builders<SessionHistory>.Update;

        var combinedFilter = filter.Eq(session => session.UserName, userName)
                        & filter.Lt(session => session.ExpiryDateTime, DateTime.UtcNow);


        try
        {
            var result =
                await _sessionCollection.UpdateOneAsync(
                                            combinedFilter,
                                            update.Combine(
                                                update.Set(session => session.SessionToken, sessionToken),
                                                update.Set(session => session.CreatedDateTime, DateTime.UtcNow),
                                                update.Set(session => session.ExpiryDateTime, DateTime.UtcNow.AddMinutes(30))
                                            )
                                        );

            if (result.ModifiedCount == 0)
            {
                await _sessionCollection.InsertOneAsync(new SessionHistory()
                {
                    UserName = userName,
                    CreatedDateTime = DateTime.UtcNow,
                    ExpiryDateTime = DateTime.UtcNow.AddMinutes(30),
                    SessionToken = sessionToken
                });
            }
            return sessionToken;
        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while creating session token for user: {username}. See Erro below:\n {error}",
                userName,
                error
            );
            return null;
        }



    }



    private async Task DeleteUserData(string UserName)
    {
        _logger.LogInformation("Deleting saved user data from User Collection");
        await _userCollection.DeleteOneAsync(data => data.UserName == UserName);
        _logger.LogInformation("Successfully deleted saved user data from User Collection");
    }

    public async Task<SessionHistory?> GetSessionHistoryFromToken(string sessionToken)
    {
        try
        {
            _logger.LogInformation("Fetching session details from token: {token}", sessionToken[..6]);

            var result = await _sessionCollection.FindAsync(
                                                    session => session.SessionToken == sessionToken &&
                                                    session.ExpiryDateTime > DateTime.UtcNow
                                                );
            _logger.LogInformation("Fetched session details successfully");

            return result.FirstOrDefault();
        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while fetching session details from token: {token}, see error satck below: \n {error}",
                sessionToken[..6],
                error
            );
            return null;
        }
    }

    public async Task<User?> GetUserDetailsFromUserName(string userName)
    {

        try
        {
            _logger.LogInformation("Fetching user details for userName: {username}", userName);

            var result = await _userCollection.FindAsync(user => user.UserName == userName);

            _logger.LogInformation("Successfully fetched user details for userName: {username}", userName);

            return result.FirstOrDefault();
        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while fetching user details for user: {userName}, see error satck below: \n {error}",
                userName,
                error
            );
            return null;
        }

    }
}