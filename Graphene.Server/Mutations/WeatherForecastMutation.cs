using Graphene.Server.Models;
using HotChocolate.Subscriptions;

namespace Graphene.Server.Mutations;

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

        weatherForecastRepository.AddWeatherForecast(weatherForecast);
        await eventSender.SendAsync("WeatherForecastAdded", weatherForecast);
        return weatherForecast;
    }
}