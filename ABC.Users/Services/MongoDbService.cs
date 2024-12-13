using ABC.Users.DTO;
using ABC.Users.Models;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ABC.Users.Services;

public class MongoDbService
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<UserAuth> _userAuthCollection;
    private readonly IMapper _mapper;


    public MongoDbService(IOptions<UsersDatabaseSettings> settings, IMapper mapper)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var userDb = mongoClient.GetDatabase(settings.Value.DatabaseName);

        _userCollection = userDb.GetCollection<User>("Users");
        _userAuthCollection = userDb.GetCollection<UserAuth>("UsersAuth");
        _mapper = mapper;

        AddIndexToUserAuthCollection();
        AddIndexToUserCollection();

    }

    private void AddIndexToUserAuthCollection()
    {
        var indexModel =
            new CreateIndexModel<UserAuth>(
                Builders<UserAuth>.IndexKeys.Ascending(m => m.UserName),
                new CreateIndexOptions() { Unique = true }
            );
        _userAuthCollection.Indexes.CreateOne(indexModel);
    }

    private void AddIndexToUserCollection()
    {
        var indexModel =
            new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(m => m.UserName),
                new CreateIndexOptions() { Unique = true }
            );
        _userCollection.Indexes.CreateOne(indexModel);
    }

    public async Task<List<User>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();
    public async Task AddUserAsync(UserSignUpDto signUpRequest)
    {
        var userData = _mapper.Map<User>(signUpRequest);

        var authData = new UserAuth()
        {
            UserName = signUpRequest.UserName,
            FirstName = signUpRequest.FirstName,
            AuthHash = signUpRequest.Password,
            Access = ["Emplyee"],
            Active = true
        };

        await _userAuthCollection.InsertOneAsync(authData);

        await _userCollection.InsertOneAsync(userData);
    }
}