using HotChocolate.Data;
using MongoDB.Driver;

namespace Graphene.Server.Queries;

[QueryType]
public class MoviesQuery
{
    [UsePaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IExecutable<Movie> GetMovies([Service] IMongoCollection<Movie> collection)
    {
        return collection.AsExecutable();
    }
    
    [UseFirstOrDefault]
    public IExecutable<Movie> GetMoviesById(
        [Service] IMongoCollection<Movie> collection,
        Guid id)
    {
        return collection.Find(x => x.Id == id).AsExecutable();
    }
    
    public IExecutable<Movie> GetMoviesByTitle(
        [Service] IMongoCollection<Movie> collection,
        string title)
    {
        return collection.Find(x => x.Title == title).AsExecutable();
    }
}

public class MovieNodeResolver
{
    public Task<Movie> ResolveAsync(
        [Service] IMongoCollection<Movie> collection,
        Guid id)
    {
        return collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}