using ABC.Accessories.Constants;
using ABC.Accessories.Models.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ABC.Accessories.Services.MongoDb;

public class MongoDbService : IMongoDbService
{

    private readonly Dictionary<string, IMongoCollection<AccessoryExtras>> _extrasCollectionMap = [];


    public MongoDbService(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var accessoryDb = mongoClient.GetDatabase(settings.Value.DatabaseName);

        AccessoriesType.AllowedTypes.ToList().ForEach(x =>
        {
            _extrasCollectionMap.Add(x, accessoryDb.GetCollection<AccessoryExtras>($"{x}Extras"));
            AddIndexToExtrasCollection(x);
        }
        );

    }

    private void AddIndexToExtrasCollection(string type)
    {
        var indexModel =
            new CreateIndexModel<AccessoryExtras>(
                Builders<AccessoryExtras>.IndexKeys.Ascending(m => m.AccessoryGuid),
                new CreateIndexOptions() { Unique = true }
            );
        _extrasCollectionMap[type].Indexes.CreateOne(indexModel);
    }

    public Dictionary<string, IMongoCollection<AccessoryExtras>> GetExtrasCollectionMap()
     => _extrasCollectionMap;
    
}