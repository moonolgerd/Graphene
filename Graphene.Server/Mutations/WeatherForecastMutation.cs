using Dapr.Actors;
using Dapr.Actors.Client;
using Graphene.Server.Actors;
using Graphene.Server.Models;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace Graphene.Server.Mutations;

[Authorize]
[MutationType]
public class WeatherForecastMutation
{
    public WeatherForecastMutation()
    {
    }

    public async Task<WeatherForecast> AddWeatherForecast(WeatherForecastInput weatherForecastInput, [Service] ITopicEventSender eventSender,
        [Service] IActorProxyFactory actorProxyFactory)
    {
        WeatherForecast weatherForecast = new(
            weatherForecastInput.Date,
            weatherForecastInput.TemperatureC,
            weatherForecastInput.Summary);

        var actor = actorProxyFactory.CreateActorProxy<IWeatherForecastActor>(new ActorId(weatherForecast.Date), nameof(WeatherForecastActor));

        await actor.AddWeatherForecast(weatherForecast);
        await eventSender.SendAsync("WeatherForecastAdded", weatherForecast);
        return weatherForecast;
    }

    public async Task UpdateWeatherForecast(WeatherForecastInput weatherForecastInput,
        [Service] IActorProxyFactory actorProxyFactory)
    {
        var actor = actorProxyFactory.CreateActorProxy<IWeatherForecastActor>(new ActorId(weatherForecastInput.Date), nameof(WeatherForecastActor));

        await actor.UpdateWeatherForecast(weatherForecastInput);
    }

    public async Task DeleteWeatherForecast(string id,
        [Service] IActorProxyFactory actorProxyFactory)
    {
        var actor = actorProxyFactory.CreateActorProxy<IWeatherForecastActor>(new ActorId(id), nameof(WeatherForecastActor));

        await actor.DeleteWeatherForecast(id);
    }
}