using Redis.OM.Modeling;

namespace Graphene.Server.Models;

public record WeatherForecast(string Date, int TemperatureC, string? Summary) : WeatherForecastInput(Date, TemperatureC, Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record WeatherForecastInput(string Date, int TemperatureC, string? Summary);