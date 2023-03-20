namespace Graphene.Server.Models
{
    public interface IWeatherForecastRepository
    {
        void AddWeatherForecast(WeatherForecast weatherForecast);
        IEnumerable<WeatherForecast> GetWeatherForecasts();
    }
}