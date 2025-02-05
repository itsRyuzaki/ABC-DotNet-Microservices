using ABC.Users.Models;
using MongoDB.Driver;

namespace ABC.Users.Services;

public interface IMongoDBService
{
    public IMongoCollection<User> GetUserCollection();

    public IMongoCollection<UserAuth> GetUserAuthCollection();

    public IMongoCollection<SessionHistory> GetSessionHistoryCollection();

}