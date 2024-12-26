using System.Text;
using ABC.Users.DTO.Request;
using ABC.Users.DTO.Response;
using ABC.Users.Enums;
using ABC.Users.Models;
using ABC.Users.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ABC.Users.Facade;

public class UserFacade
            (
                IMapper _mapper,
                IUserService _userService,
                IConfiguration config,
                ILogger<UserFacade> _logger
            ) : IUserFacade
{
    public async Task<ApiResponseDto<UserResponseDTO>> LoginUserAsync(UserLoginDto loginRequest)
    {

        var result = await _userService.GetUserAuthAsync(loginRequest.UserName);

        if (result == null)
        {
            _logger.LogInformation("No such user with username: {username} exists in userAuth!", loginRequest.UserName);
            return ApiResponseDto<UserResponseDTO>.HandleErrorResponse(
                            (int)ResponseCode.NOT_FOUND,
                            ["No Such user exists"]
                        );
        }

        bool validCreds = result.AuthHash.SequenceEqual(
                                                CreateAuthHash(loginRequest.Password, result.AuthSalt)
                                            );

        if (validCreds)
        {
            _logger.LogInformation("Credentials matched for user");

            var userDetails = await _userService.GetUserDetailsFromUserName(loginRequest.UserName);

            if (userDetails == null)
            {
                _logger.LogInformation("No such user with username: {username} exists in User!", loginRequest.UserName);
                return ApiResponseDto<UserResponseDTO>.HandleErrorResponse(
                                (int)ResponseCode.NOT_FOUND,
                                ["No Such user exists"]
                            );
            }
            else
            {
                return ApiResponseDto<UserResponseDTO>
                                .HandleSuccessResponse(_mapper.Map<UserResponseDTO>(userDetails));
            }

        }
        else
        {
            _logger.LogInformation("Invalid Credentials provided for user login");

            return ApiResponseDto<UserResponseDTO>.HandleErrorResponse(
                                            (int)ResponseCode.UNAUTHORIZED,
                                            ["Wrong username or password"]
                                        );
        }
    }

    public async Task<ApiResponseDto<string>> SignUpUserAsync(UserSignUpDto signUpRequest)
    {
        var userData = _mapper.Map<User>(signUpRequest);
        userData.CreatedDateTime = DateTime.UtcNow;
        userData.UpdatedDateTime = null;
        userData.LastLoginDateTime = DateTime.UtcNow;
        userData.AccessibleModules = ["Customer"];

        UserAuth authData = CreateUserAuthData(signUpRequest);

        // Add user data first
        var userAddedResponse = await _userService.InsertUserDataAsync(userData);

        // if success then add in userAuth table
        if (userAddedResponse.Success)
        {
            return await _userService.InsertUserAuthDataAsync(authData);
        }
        else
        {
            return userAddedResponse;
        }
    }

    public async Task<string?> CreateSessionHistoryAsync(string userName)
    {
        return await _userService.GetAndSetSessionToken(userName);
    }

    private UserAuth CreateUserAuthData(UserSignUpDto signUpRequest)
    {
        var salt = Guid.NewGuid().ToString();

        return new UserAuth()
        {
            UserName = signUpRequest.UserName,
            AuthHash = CreateAuthHash(signUpRequest.Password, salt),
            AuthSalt = salt,
        };
    }

    private byte[] CreateAuthHash(string password, string salt)
    {
        return KeyDerivation.Pbkdf2(
            password,
            Encoding.ASCII.GetBytes(salt + config.GetSection("AppSettings:SaltKey")),
            KeyDerivationPrf.HMACSHA256,
            100,
            64
        );
    }

    public async Task<ApiResponseDto<UserResponseDTO>> GetUserDetailsFromSessionAsync(string sessionToken)
    {
        var sessionDetails = await _userService.GetSessionHistoryFromToken(sessionToken);


        if (sessionDetails?.UserName != null)
        {
            var userDetails = await _userService.GetUserDetailsFromUserName(sessionDetails.UserName);
            if (userDetails != null)
            {
                return ApiResponseDto<UserResponseDTO>
                                 .HandleSuccessResponse(_mapper.Map<UserResponseDTO>(userDetails));
            }
        }

        return ApiResponseDto<UserResponseDTO>.HandleErrorResponse(
                                (int)ResponseCode.UNAUTHORIZED,
                                ["No saved credentials were found"]
                            );
    }
}