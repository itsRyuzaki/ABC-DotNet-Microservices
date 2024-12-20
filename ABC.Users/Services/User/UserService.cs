using ABC.Users.DTO;
using ABC.Users.Models;
using AutoMapper;
using MongoDB.Driver;

namespace ABC.Users.Services;

public class UserService(IMongoDBService mongoService, IMapper _mapper) : IUserService
{
    private readonly IMongoCollection<User> _userCollection = mongoService.GetUserCollection();
    private readonly IMongoCollection<UserAuth> _userAuthCollection = mongoService.GetUserAuthCollection();

    public async Task AddUserAsync(UserSignUpDto signUpRequest)
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

        await _userAuthCollection.InsertOneAsync(authData);

        await _userCollection.InsertOneAsync(userData);
    }

}