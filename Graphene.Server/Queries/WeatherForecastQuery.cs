using Graphene.Server.Models;
using HotChocolate.AspNetCore.Authorization;

namespace Graphene.Server.Queries;

public class WeatherForecastQuery
{
    private readonly WeatherForecastRepository weatherForecastRepository;
    private readonly ILogger<WeatherForecastQuery> logger;

    public WeatherForecastQuery(WeatherForecastRepository weatherForecastRepository,
        ILogger<WeatherForecastQuery> logger)
    {
        this.weatherForecastRepository = weatherForecastRepository;
        this.logger = logger;
    }

    [Authorize]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        logger.LogInformation("Getting weather forecasts");
        return weatherForecastRepository.GetWeatherForecasts();
    }
}