using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Users.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    public required string UserName { get; set; }

    [BsonRequired]

    public required string FirstName { get; set; }

    public string? LastName { get; set; }

    [BsonRequired]

    public required string EmailId { get; set; }

    [BsonRequired]

    public required string MobileNumber { get; set; }

    public Address? UserAddress { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? UpdatedDateTime { get; set; }
    public DateTime LastLoginDateTime { get; set; }

}
