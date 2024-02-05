using Redis.OM.Modeling;

namespace Graphene.Server.Models;

[Document(StorageType = StorageType.Json, Prefixes = ["Person"])]
public record Person
{
    [RedisIdField][Indexed] public string? Id { get; set; }

    [Indexed] public string? FirstName { get; set; }

    [Indexed] public string? LastName { get; set; }

    [Indexed] public int Age { get; set; }

    [Searchable] public string? PersonalStatement { get; set; }

    [Indexed] public string[] Skills { get; set; } = [];

    [Indexed(CascadeDepth = 1)] public Address? Address { get; set; }

}

public record Address
{
    [Indexed]
    public int? StreetNumber { get; set; }

    [Searchable]
    public string? StreetName { get; set; }

    [Indexed]
    public string? City { get; set; }

    [Indexed]
    public string? State { get; set; }

    [Indexed]
    public string? PostalCode { get; set; }

    [Indexed]
    public string? Country { get; set; }
}
