using Graphene.Server.Models;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace Graphene.Server.Mutations;

[Authorize]
[MutationType]
public class WeatherForecastMutation
{
    private readonly WeatherForecastRepository weatherForecastRepository;

    public WeatherForecastMutation(WeatherForecastRepository weatherForecastRepository)
    {
        this.weatherForecastRepository = weatherForecastRepository;
    }

    public async Task<WeatherForecast> AddWeatherForecast(WeatherForecastInput weatherForecastInput, [Service] ITopicEventSender eventSender)
    {
        WeatherForecast weatherForecast = new(
            weatherForecastInput.Date,
            weatherForecastInput.TemperatureC,
            weatherForecastInput.Summary);

        await weatherForecastRepository.AddWeatherForecast(weatherForecast);
        await eventSender.SendAsync("WeatherForecastAdded", weatherForecast);
        return weatherForecast;
    }

    public async Task UpdateWeatherForecast(WeatherForecastInput weatherForecastInput)
    {
        await weatherForecastRepository.UpdateWeatherForecast(weatherForecastInput);
    }
}