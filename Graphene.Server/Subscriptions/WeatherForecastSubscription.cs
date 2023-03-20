using Graphene.Server.Models;
using HotChocolate.Authorization;

namespace Graphene.Server.Subscriptions;

[Authorize]
[SubscriptionType]
public class WeatherForecastSubscription
{
    [Subscribe]
    [Topic("WeatherForecastAdded")]
    public WeatherForecast OnNewWeatherForecast([EventMessage] WeatherForecast weatherForecast) => weatherForecast;
}