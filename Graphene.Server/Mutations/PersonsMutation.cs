using Graphene.Server.Models;
using Redis.OM;

namespace Graphene.Server.Mutations;

[MutationType]
public class PersonsMutation
{
    public async Task<Person> AddPerson(
        PersonInput personInput,
        [Service] RedisConnectionProvider provider)
    {
        var collection = provider.RedisCollection<Person>();
        var person = new Person
        {
            Id = $"{personInput.FirstName}:{personInput.LastName}",
            FirstName = personInput.FirstName,
            LastName = personInput.LastName,
            Age = personInput.Age,
            Address = new Address
            {
                City = personInput.City,
                StreetName = personInput.StreetName,
                StreetNumber = personInput.StreetNumber
            }
        };

        await collection.InsertAsync(person);
        return person;
    }

    public async Task<Person?> UpdatePerson(
         UpdatePersonInput personInput,
        [Service] RedisConnectionProvider provider)
    {
        var collection = provider.RedisCollection<Person>();
        var person = await collection.FindByIdAsync(personInput.Id);
        if (person == null)
            return null;

        person.Age = personInput.Age;
        await collection.SaveAsync();

        return person;
    }

    public async Task DeletePerson(string Id,
        [Service] RedisConnectionProvider provider)
    {
        var collection = provider.RedisCollection<Person>();
        var person = await collection.FindByIdAsync(Id);

        if (person == null)
            return;

        await collection.DeleteAsync(person);

        
    }
}

public record PersonInput(string FirstName,
    string LastName, int Age)
{
    public int? StreetNumber { get; init; }
    public string? StreetName { get; init; }
    public string? City { get; init; }
}

public record UpdatePersonInput(string Id, int Age);