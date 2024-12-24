using System.Security.Cryptography;
using System.Text;
using ABC.Users.DTO.Request;
using ABC.Users.DTO.Response;
using ABC.Users.Enums;
using ABC.Users.Models;
using ABC.Users.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ABC.Users.Facade;

public class UserFacade(IMapper _mapper, IUserService _userService, IConfiguration config, ILogger<UserFacade> _logger) : IUserFacade
{
    public async Task<UserResponseDTO> LoginUserAsync(UserLoginDto loginRequest)
    {

        var result = await _userService.GetUserAuthAsync(loginRequest.UserName);

        if (result == null)
        {
            _logger.LogInformation("No such user with username: {username} exists!", loginRequest.UserName);
            return _mapper.Map<UserResponseDTO>(
                                    ApiResponseDto.HandleErrorResponse(
                                            (int)ResponseCode.NOT_FOUND,
                                            ["No Such user exists"]
                                        )
                                    );
        }

        bool validCreds = result.AuthHash.SequenceEqual(
                                                CreateAuthHash(loginRequest.Password, result.AuthSalt)
                                            );

        if (validCreds)
        {
            _logger.LogInformation("Credentials matched for user");
            return new UserResponseDTO()
            {
                FirstName = result.FirstName,
                AvatarUrl = result.AvatarUrl,
                AccessibleModules = result.AccessibleModules,
                Success = true,
            };
        }
        else
        {
            _logger.LogInformation("Invalid Credentials provided for user login");

            return _mapper.Map<UserResponseDTO>(
                                    ApiResponseDto.HandleErrorResponse(
                                            (int)ResponseCode.BAD_REQUEST,
                                            ["Wrong username or password"]
                                        )
                                    );
        }
    }

    public async Task<ApiResponseDto> SignUpUserAsync(UserSignUpDto signUpRequest)
    {
        var userData = _mapper.Map<User>(signUpRequest);
        userData.CreatedDateTime = DateTime.UtcNow;
        userData.UpdatedDateTime = null;
        userData.LastLoginDateTime = DateTime.UtcNow;

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

    public async Task<string> CreateSessionHistory(string userName)
    {
        return await _userService.GetSessionToken(userName);
    }

    private UserAuth CreateUserAuthData(UserSignUpDto signUpRequest)
    {
        var salt = Guid.NewGuid().ToString();

        return new UserAuth()
        {
            UserName = signUpRequest.UserName,
            FirstName = signUpRequest.FirstName,
            AuthHash = CreateAuthHash(signUpRequest.Password, salt),
            AuthSalt = salt,
            AccessibleModules = ["Customer"],
            Active = true
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
}