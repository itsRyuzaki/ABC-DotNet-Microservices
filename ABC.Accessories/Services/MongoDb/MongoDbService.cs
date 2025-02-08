using ABC.Accessories.Constants;
using ABC.Accessories.Models.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ABC.Accessories.Services.MongoDb;

public class MongoDbService : IMongoDbService
{

    private readonly Dictionary<string, IMongoCollection<AccessoryExtras>> _accExtrasCollectionMap = [];
    private readonly Dictionary<string, IMongoCollection<AccessoryBaseExtras>> _baseExtrasCollectionMap = [];


    public MongoDbService(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var accessoryDb = mongoClient.GetDatabase(settings.Value.DatabaseName);

        AccessoriesType.AllowedTypes.ToList().ForEach(x =>
        {
            _accExtrasCollectionMap.Add(x, accessoryDb.GetCollection<AccessoryExtras>($"{x}Extras"));
            AddIndexToAccessoryExtrasCollection(x);

            _baseExtrasCollectionMap.Add(x, accessoryDb.GetCollection<AccessoryBaseExtras>($"{x}BaseExtras"));
            AddIndexToAccessoryBaseExtrasCollection(x);
        }
        );

    }

    private void AddIndexToAccessoryExtrasCollection(string type)
    {
        var indexModel =
            new CreateIndexModel<AccessoryExtras>(
                Builders<AccessoryExtras>.IndexKeys.Ascending(m => m.AccessoryGuid),
                new CreateIndexOptions() { Unique = true }
            );
        _accExtrasCollectionMap[type].Indexes.CreateOne(indexModel);
    }

    private void AddIndexToAccessoryBaseExtrasCollection(string type)
    {
        var indexModel =
            new CreateIndexModel<AccessoryBaseExtras>(
                Builders<AccessoryBaseExtras>.IndexKeys.Ascending(m => m.AccessoryBaseId),
                new CreateIndexOptions() { Unique = true }
            );
        _baseExtrasCollectionMap[type].Indexes.CreateOne(indexModel);
    }

    public Dictionary<string, IMongoCollection<AccessoryExtras>> GetAccExtrasCollectionMap()
     => _accExtrasCollectionMap;

      public Dictionary<string, IMongoCollection<AccessoryBaseExtras>> GetBaseExtrasCollectionMap()
     => _baseExtrasCollectionMap;
    
}