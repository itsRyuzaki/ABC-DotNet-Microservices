using System.Security.Cryptography;
using System.Text;
using ABC.Users.DTO;
using ABC.Users.Models;
using ABC.Users.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ABC.Users.Facade;

public class UserFacade(IMapper _mapper, IUserService _userService, IConfiguration config) : IUserFacade
{
    public async Task<ApiResponseDto> LoginUserAsync(UserLoginDto loginRequest)
    {
        var result = await _userService.GetUserAuthAsync(loginRequest.UserName);

        return new ApiResponseDto()
        {
            Success = result.AuthHash.SequenceEqual(CreateAuthHash(loginRequest.Password, result.AuthSalt))
        };
    }

    public async Task<ApiResponseDto> SignUpUserAsync(UserSignUpDto signUpRequest)
    {
        var userData = _mapper.Map<User>(signUpRequest);
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

    private UserAuth CreateUserAuthData(UserSignUpDto signUpRequest)
    {
        var salt = Guid.NewGuid().ToString();

        return new UserAuth()
        {
            UserName = signUpRequest.UserName,
            FirstName = signUpRequest.FirstName,
            AuthHash = CreateAuthHash(signUpRequest.Password, salt),
            AuthSalt = salt,
            AccessibleModules = ["Employee"],
            Active = true
        };
    }

    private byte[] CreateAuthHash(string password, string salt)
    {
        return KeyDerivation.Pbkdf2(
            password,
            Encoding.ASCII.GetBytes(salt + config.GetSection("AppSettings:extraSaltKey")),
            KeyDerivationPrf.HMACSHA256,
            100,
            32
        );
    }
}