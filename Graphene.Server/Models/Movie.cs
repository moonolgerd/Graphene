namespace Graphene.Server.Queries;

[Node(
    IdField = nameof(Id),
    NodeResolverType = typeof(MovieNodeResolver),
    NodeResolver = nameof(MovieNodeResolver.ResolveAsync))]
public record Movie(Guid Id, string Title, int Year, string Rated, int Runtime, string[] Directors, string[] Genres,
    string[] Cast);