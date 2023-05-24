using Graphene.Server.Models;
using Redis.OM;

namespace Graphene.Server.Queries;

[QueryType]
public class PersonsQuery
{
    public async Task<IEnumerable<Person>> GetPeopleAsync(
        [Service] RedisConnectionProvider provider)
    {
        var collection = provider.RedisCollection<Person>();
        var list = await collection.ToListAsync();
        return list;
    }
}
