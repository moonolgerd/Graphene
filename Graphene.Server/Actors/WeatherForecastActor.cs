using Dapr.Actors;
using Dapr.Actors.Runtime;
using Graphene.Server.Models;

namespace Graphene.Server.Actors;

public class WeatherForecastActor : Actor, IWeatherForecastActor
{
    public WeatherForecastActor(ActorHost host) : base(host)
    {
        Logger.LogInformation("WeatherForecastActor created with Id {Id}", Id);
    }

    public async Task AddWeatherForecast(WeatherForecastInput weatherForecastInput)
    {
        Logger.LogInformation("Adding WeatherForecast with Id {Id}", weatherForecastInput.Date);

        var weatherForecast = new WeatherForecast(weatherForecastInput.Date, weatherForecastInput.TemperatureC, weatherForecastInput.Summary);
        await StateManager.AddStateAsync(weatherForecast.Date, weatherForecast);
    }

    public async Task DeleteWeatherForecast(string id)
    {
        Logger.LogInformation("Deleting WeatherForecast with Id {Id}", Id);
        await StateManager.RemoveStateAsync(id);
    }

    public async Task UpdateWeatherForecast(WeatherForecastInput weatherForecastInput)
    {
        var forecast = await StateManager.GetStateAsync<WeatherForecast>(weatherForecastInput.Date)
            ?? throw new ArgumentException($"WeatherForecast with Id {weatherForecastInput.Date} not found");

        Logger.LogInformation("Updating WeatherForecast with Id {Id}", weatherForecastInput.Date);

        var (date, temperatureC, summary) = weatherForecastInput;

        var weatherForecast = new WeatherForecast(date, temperatureC, summary);

        if (weatherForecast != forecast)
        {
            await StateManager.SetStateAsync(weatherForecast.Date, forecast);
        }
    }

    public Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(int count)
    {
        var rng = new Random();
        return Task.FromResult(Enumerable.Range(1, count).Select(index => new WeatherForecast
                   (
                           DateTime.Now.AddDays(index).ToShortDateString(),
                                          rng.Next(-20, 55),
                                                         new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" }
                                                         [rng.Next(0, 9)]
                                                                    )));
    }
}

public interface IWeatherForecastActor : IActor
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(int count);
    /// <summary>
    /// Asynchronously adds a new weather forecast to the state manager.
    /// </summary>
    /// <param name="weatherForecastInput">The input data for the new weather forecast.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddWeatherForecast(WeatherForecastInput weatherForecastInput);
    Task DeleteWeatherForecast(string id);
    /// <summary>
    /// Update the WeatherForecast with the given Id
    /// </summary>
    /// <param name="weatherForecastInput"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    Task UpdateWeatherForecast(WeatherForecastInput weatherForecastInput);
}
