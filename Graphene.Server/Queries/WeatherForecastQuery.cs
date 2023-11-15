using Graphene.Server.Models;
using HotChocolate.Authorization;

namespace Graphene.Server.Queries;

[QueryType]
[Authorize]
public class WeatherForecastQuery(IWeatherForecastRepository weatherForecastRepository,
    ILogger<WeatherForecastQuery> logger)
{
    private readonly IWeatherForecastRepository weatherForecastRepository = weatherForecastRepository;
    private readonly ILogger<WeatherForecastQuery> logger = logger;

    public Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
    {
        logger.LogInformation("Getting weather forecasts");
        return weatherForecastRepository.GetWeatherForecasts();
    }
}