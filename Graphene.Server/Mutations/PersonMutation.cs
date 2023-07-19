using Graphene.Server.Models;

namespace Graphene.Server.Mutations;

[MutationType]
public class PersonMutation
{
    public async Task<Person> AddPerson(
        PersonInput personInput,
        [Service] IPersonRepository personRepository)
    {
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

        await personRepository.AddPerson(person);
        return person;
    }

    public async Task<Person?> UpdatePerson(
         UpdatePersonInput personInput,
        [Service] IPersonRepository personRepository)
    {
        var person = await personRepository.GetPersonById(personInput.Id);
        if (person == null)
            return null;

        person.Age = personInput.Age;
        await personRepository.UpdatePerson(person);

        return person;
    }

    public async Task DeletePerson(
        string Id,
        [Service] IPersonRepository personRepository)
    {
        var person = await personRepository.GetPersonById(Id);

        if (person == null)
            return;

        await personRepository.DeletePerson(person);
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