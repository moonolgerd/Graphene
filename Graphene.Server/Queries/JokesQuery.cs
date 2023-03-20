using HotChocolate.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Graphene.Server.Queries
{
    [QueryType]
    [Authorize]
    public class JokesQuery
    {
        public Joke GetJoke([GlobalState("UserId")] string userId, [Service] IMemoryCache memoryCache)
        {
            return memoryCache.Get<Joke>("joke");
        }
    }
}
