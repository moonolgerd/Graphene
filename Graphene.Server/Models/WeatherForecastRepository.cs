using Redis.OM;

namespace Graphene.Server.Models;

public class WeatherForecastRepository : IWeatherForecastRepository
{
  readonly string[] summaries = new[]
  {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  private readonly List<WeatherForecast> weatherForecasts = new();
  private readonly RedisConnectionProvider redisConnectionProvider;

  public WeatherForecastRepository(RedisConnectionProvider redisConnectionProvider)
  {
    weatherForecasts = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index).ToString(),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToList();
    this.redisConnectionProvider = redisConnectionProvider;

    foreach (var weatherForecast in weatherForecasts)
      redisConnectionProvider.RedisCollection<WeatherForecast>().Insert(weatherForecast);
  }

  public IEnumerable<WeatherForecast> GetWeatherForecasts() => redisConnectionProvider.RedisCollection<WeatherForecast>().ToList();

  public Task<WeatherForecast?> GetWeatherForecast(string date)
  {
    var weatherForecast = redisConnectionProvider.RedisCollection<WeatherForecast>().FindByIdAsync(date);
    return weatherForecast;
  }

  public async Task AddWeatherForecast(WeatherForecastInput weatherForecastInput)
  {
    var weatherForecast = new WeatherForecast(weatherForecastInput.Date, weatherForecastInput.TemperatureC, weatherForecastInput.Summary);
    await redisConnectionProvider.RedisCollection<WeatherForecast>().InsertAsync(weatherForecast);
  }

  public async Task UpdateWeatherForecast(WeatherForecastInput weatherForecastInput)
  {
    var weatherForecast = new WeatherForecast(weatherForecastInput.Date, weatherForecastInput.TemperatureC, weatherForecastInput.Summary);
    await redisConnectionProvider.RedisCollection<WeatherForecast>().UpdateAsync(weatherForecast);
  }

  public async Task DeleteWeatherForecast(string date)
  {
    var weatherForecast = await redisConnectionProvider.RedisCollection<WeatherForecast>().FindByIdAsync(date);
    await redisConnectionProvider.RedisCollection<WeatherForecast>().DeleteAsync(weatherForecast!);
  }
}
