using Graphene.Server.Queries;
using MongoDB.Driver;

namespace Graphene.Server.Mutations;

[MutationType]
public class MoviesMutation
{
    public async Task<CreateMoviePayload> CreatePersonAsync(
        [Service] IMongoCollection<Movie> collection,
        CreateMovieInput input)
    {
        var movie = new Movie(Guid.NewGuid(), input.Title, input.Year, input.Rated,
            input.Runtime, input.Directors.ToArray(), input.Genres.ToArray(),
            input.Cast.ToArray());

        await collection.InsertOneAsync(movie);

        return new CreateMoviePayload(movie);
    }
}

public record CreateMoviePayload(Movie Movie);

public record CreateMovieInput(
    string Title,
    int Year,
    string Rated,
    int Runtime,
    IReadOnlyList<string> Directors,
    IReadOnlyList<string> Genres,
    IReadOnlyList<string> Cast);