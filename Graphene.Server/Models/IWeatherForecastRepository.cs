namespace Graphene.Server.Models
{
  public interface IWeatherForecastRepository
  {
    Task AddWeatherForecast(WeatherForecastInput weatherForecast);
    Task UpdateWeatherForecast(WeatherForecastInput weatherForecast);
    IEnumerable<WeatherForecast> GetWeatherForecasts();
  }
}