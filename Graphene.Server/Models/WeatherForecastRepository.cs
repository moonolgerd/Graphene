namespace Graphene.Server.Models;

public class WeatherForecastRepository
{
    readonly string[] summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly List<WeatherForecast> weatherForecasts = new();

    public WeatherForecastRepository()
    {
        weatherForecasts = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateTime.Now.AddDays(index).ToString(),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToList();
    }

    public IEnumerable<WeatherForecast> GetWeatherForecasts() => weatherForecasts;

    public void AddWeatherForecast(WeatherForecast weatherForecast)
    {
        weatherForecasts.Add(weatherForecast);
    }
}
