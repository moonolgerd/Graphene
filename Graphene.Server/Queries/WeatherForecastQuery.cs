using Graphene.Server.Models;
using HotChocolate.Authorization;

namespace Graphene.Server.Queries;

[QueryType]
//[Authorize]
public class WeatherForecastQuery(ILogger<WeatherForecastQuery> logger)
{
    private readonly ILogger<WeatherForecastQuery> logger = logger;

    public Task<IEnumerable<WeatherForecast>> GetWeatherForecast([Service] IWeatherForecastRepository weatherForecastRepository)
    {
        logger.LogInformation("Getting weather forecasts");
        return weatherForecastRepository.GetWeatherForecasts();
    }
}