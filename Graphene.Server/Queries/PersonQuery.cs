using Graphene.Server.Models;

namespace Graphene.Server.Queries;

[QueryType]
public class PersonQuery
{
    public async Task<IEnumerable<Person>> GetPeopleAsync(
        [Service] IPersonRepository personRepository) => await personRepository.GetAllPeople();
}
