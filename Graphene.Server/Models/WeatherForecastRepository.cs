using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Graphene.Server.Models;

public class WeatherForecastRepository : IWeatherForecastRepository
{
  private readonly IRedisCollection<WeatherForecast> redisCollection;

  public WeatherForecastRepository(IRedisConnectionProvider redisConnectionProvider)
    => redisCollection = redisConnectionProvider.RedisCollection<WeatherForecast>();

  public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts() => await redisCollection.ToListAsync();

  public async Task<WeatherForecast?> GetWeatherForecast(string date)
  {
    var weatherForecast = await redisCollection.FindByIdAsync(date);
    return weatherForecast;
  }

  public async Task AddWeatherForecast(WeatherForecastInput weatherForecastInput)
  {
    var weatherForecast = new WeatherForecast(weatherForecastInput.Date, weatherForecastInput.TemperatureC, weatherForecastInput.Summary);
    await redisCollection.InsertAsync(weatherForecast);
  }

  public async Task UpdateWeatherForecast(WeatherForecastInput weatherForecastInput)
  {
    var weatherForecast = new WeatherForecast(weatherForecastInput.Date, weatherForecastInput.TemperatureC, weatherForecastInput.Summary);
    await redisCollection.UpdateAsync(weatherForecast);
  }

  public async Task DeleteWeatherForecast(string date)
  {
    var weatherForecast = await redisCollection.FindByIdAsync(date);
    await redisCollection.DeleteAsync(weatherForecast!);
  }
}
