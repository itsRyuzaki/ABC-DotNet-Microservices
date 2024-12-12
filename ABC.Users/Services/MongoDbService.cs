using ABC.Users.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ABC.Users.Services;

public class MongoDbService
{
    private readonly IMongoCollection<User> _userCollection;

    public MongoDbService(IOptions<UsersDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var userDb = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _userCollection = userDb.GetCollection<User>("users");

    }

    public async Task<List<User>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();
    public async Task CreateAsync(User userData) =>
        await _userCollection.InsertOneAsync(userData);
}