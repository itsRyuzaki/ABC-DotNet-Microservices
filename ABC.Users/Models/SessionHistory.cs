using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Users.Models;

public class SessionHistory {
    [BsonRequired]
    public required string UserName { get; set; }

    [BsonRequired]
    public required string SessionToken { get; set; }

    [BsonRequired]
    public required DateTime CreatedDateTime { get; set; }
    
    [BsonRequired]
    public required DateTime ExpiryDateTime { get; set; }
}