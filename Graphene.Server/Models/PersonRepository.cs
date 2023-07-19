using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Graphene.Server.Models;

public interface IPersonRepository
{
    /// <summary>
    /// Retrieves a list of all people.
    /// </summary>
    /// <returns>A list of Person objects.</returns>
    Task<IList<Person>> GetAllPeople();

    /// <summary>
    /// Retrieves a Person object by ID.
    /// </summary>
    /// <param name="id">The ID of the person.</param>
    /// <returns>A Person object.</returns>
    Task<Person?> GetPersonById(string id);

    /// <summary>
    /// Adds a new person.
    /// </summary>
    /// <param name="person">The Person object to add.</param>
    Task AddPerson(Person person);

    /// <summary>
    /// Updates an existing person.
    /// </summary>
    /// <param name="person">The updated Person object.</param>
    Task UpdatePerson(Person person);

    /// <summary>
    /// Deletes a person.
    /// </summary>
    /// <param name="person">The Person object to delete.</param>
    Task DeletePerson(Person person);
}


public class PersonRepository: IPersonRepository
{
    private readonly IRedisCollection<Person> redisCollection;

    public PersonRepository(IRedisConnectionProvider redisConnectionProvider)
    {
        redisCollection = redisConnectionProvider.RedisCollection<Person>();
    }

    public async Task<IList<Person>> GetAllPeople() => await redisCollection.ToListAsync();

    public async Task<Person?> GetPersonById(string id) => await redisCollection.FindByIdAsync(id);

    public async Task AddPerson(Person person) => await redisCollection.InsertAsync(person);

    public async Task UpdatePerson(Person person) => await redisCollection.UpdateAsync(person);

    public async Task DeletePerson(Person person) => await redisCollection.DeleteAsync(person);
}
