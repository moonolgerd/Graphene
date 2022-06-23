using Graphene.Server.Models;

namespace Graphene.Server.Subscriptions;

public class WeatherForecastSubscription
{
    [Subscribe]
    [Topic("WeatherForecastAdded")]
    public WeatherForecast OnNewWeatherForecast([EventMessage] WeatherForecast weatherForecast) => weatherForecast;
}