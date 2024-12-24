using ABC.Users.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ABC.Users.Services;

public class MongoDbService : IMongoDBService
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<UserAuth> _userAuthCollection;

    private readonly IMongoCollection<SessionHistory> _sessionCollection;


    public MongoDbService(IOptions<UsersDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var userDb = mongoClient.GetDatabase(settings.Value.DatabaseName);

        _userCollection = userDb.GetCollection<User>("Users");
        _userAuthCollection = userDb.GetCollection<UserAuth>("UsersAuth");
        _sessionCollection = userDb.GetCollection<SessionHistory>("SessionHistory");


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

    public IMongoCollection<User> GetUserCollection()
    {
        return _userCollection;
    }

    public IMongoCollection<UserAuth> GetUserAuthCollection()
    {
        return _userAuthCollection;
    }

    public IMongoCollection<SessionHistory> GetSessionHistoryCollection()
    {
        return _sessionCollection;
    }
}