using Graphene.Server.Models;

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

    public IEnumerable<WeatherForecast> GetWeatherForecast() => weatherForecastRepository.GetWeatherForecasts();
}