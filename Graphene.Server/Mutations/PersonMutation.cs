using Dapr.Actors;
using Dapr.Actors.Client;
using Graphene.Server.Actors;
using Graphene.Server.Models;

namespace Graphene.Server.Mutations;

[MutationType]
public class PersonMutation
{
    public async Task<Person> AddPerson(
        PersonInput personInput,
        [Service] IActorProxyFactory actorProxyFactory)
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

        var actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(person.Id), nameof(PersonActor));
        await actor.AddPersonAsync(person);
        return person;
    }

    public async Task<Person?> UpdatePerson(
         UpdatePersonInput personInput,
        [Service] IActorProxyFactory actorProxyFactory)
    {
        var personActor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(personInput.Id), nameof(PersonActor));
        var person = await personActor.GetPersonAsync(personInput.Id);
        if (person == null)
            return null;

        person.Age = personInput.Age;
        await personActor.UpdatePersonAsync(person);

        return person;
    }

    public async Task DeletePerson(
        string Id,
        [Service] IActorProxyFactory actorProxyFactory)
    {
        var personActor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(Id), "PersonActor");
        var person = await personActor.GetPersonAsync(Id);

        if (person == null)
            return;

        await personActor.DeletePersonAsync(person.Id);
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