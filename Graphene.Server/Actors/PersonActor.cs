using Dapr.Actors;
using Dapr.Actors.Runtime;
using Graphene.Server.Models;

namespace Graphene.Server.Actors;

public class PersonActor: Actor, IPersonActor
{
    public PersonActor(ActorHost host) : base(host)
    {
    }

    public async Task<Person?> GetPersonAsync(string? id) => await StateManager.GetStateAsync<Person>(id);

    public async Task AddPersonAsync(Person person)
    {
        Logger.LogInformation("Adding Person with Id {Id}", person.Id);
        await StateManager.AddStateAsync(person.Id, person);
    }

    public async Task UpdatePersonAsync(Person person) => await StateManager.SetStateAsync(person.Id, person);

    public async Task DeletePersonAsync(string? id) => await StateManager.RemoveStateAsync(id);
}

public interface IPersonActor: IActor
{
    Task<Person?> GetPersonAsync(string? id);
    Task AddPersonAsync(Person person);
    Task UpdatePersonAsync(Person person);
    Task DeletePersonAsync(string? id);
}
