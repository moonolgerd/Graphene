using Graphene.Server.Models;
using HotChocolate.Authorization;

namespace Graphene.Server.Subscriptions;

[SubscriptionType]
public class WeatherForecastSubscription
{
    [Subscribe]
    [Topic("WeatherForecastAdded")]
    //[Authorize]
    public WeatherForecast OnNewWeatherForecast([EventMessage] WeatherForecast weatherForecast) => weatherForecast;
}