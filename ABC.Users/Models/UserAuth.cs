using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Users.Models;

public class UserAuth
{

    [BsonRequired]
    public required string UserName { get; set; }

    [BsonRequired]
    public required string FirstName { get; set; }

    [BsonRequired]
    public required string AuthHash { get; set; }

    [BsonRequired]
    public required string[] AccessibleModules { get; set; }

    [BsonRequired]
    public required bool Active { get; set; }

}