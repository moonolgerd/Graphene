using Graphene.Server.Models;
using HotChocolate.Authorization;

namespace Graphene.Server.Queries;

[QueryType]
[Authorize]
public class WeatherForecastQuery
{
    private readonly IWeatherForecastRepository weatherForecastRepository;
    private readonly ILogger<WeatherForecastQuery> logger;

    public WeatherForecastQuery(IWeatherForecastRepository weatherForecastRepository,
        ILogger<WeatherForecastQuery> logger)
    {
        this.weatherForecastRepository = weatherForecastRepository;
        this.logger = logger;
    }
        
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        logger.LogInformation("Getting weather forecasts");
        return weatherForecastRepository.GetWeatherForecasts();
    }
}