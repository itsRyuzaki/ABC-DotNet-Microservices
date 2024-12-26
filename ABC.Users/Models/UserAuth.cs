using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Users.Models;

public class UserAuth
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get; set;}

    [BsonRequired]
    public required string UserName { get; set; }

    [BsonRequired]
    public required byte[] AuthHash { get; set; }
    [BsonRequired]
    public required string AuthSalt { get; set; }

}