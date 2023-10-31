using Dapr.Actors;
using Dapr.Actors.Client;
using Graphene.Server.Actors;
using Graphene.Server.Models;
using HotChocolate.Authorization;

namespace Graphene.Server.Queries;

[QueryType]
[Authorize]
public class WeatherForecastQuery
{
    private readonly ILogger<WeatherForecastQuery> logger;

    public WeatherForecastQuery(ILogger<WeatherForecastQuery> logger)
    {
        this.logger = logger;
    }
        
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(
        [Service] IActorProxyFactory actorProxyFactory)
    {
        var weatherForecastActor = actorProxyFactory.CreateActorProxy<IWeatherForecastActor>(new ActorId("WeatherForecastActor"), nameof(WeatherForecastActor));
        logger.LogInformation("Getting weather forecasts");
        return await weatherForecastActor.GetWeatherForecastsAsync(5);
    }
}