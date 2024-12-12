using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Users.Models;

public class Address
{

    [BsonRequired]

    public required string Name { get; set; }

    [BsonRequired]

    public required string AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    [BsonRequired]
    public required string PostalCode { get; set; }

}
