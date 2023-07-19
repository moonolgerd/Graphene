namespace Graphene.Server.Models;

/// <summary>
/// Interface for a weather forecast repository.
/// </summary>
public interface IWeatherForecastRepository
{
    /// <summary>
    /// Adds a new weather forecast to the repository.
    /// </summary>
    /// <param name="weatherForecast">The weather forecast to add.</param>
    Task AddWeatherForecast(WeatherForecastInput weatherForecast);

    /// <summary>
    /// Updates an existing weather forecast in the repository.
    /// </summary>
    /// <param name="weatherForecast">The updated weather forecast.</param>
    Task UpdateWeatherForecast(WeatherForecastInput weatherForecast);

    /// <summary>
    /// Gets all weather forecasts from the repository.
    /// </summary>
    /// <returns>An enumerable collection of weather forecasts.</returns>
    Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();

    /// <summary>
    /// Gets a weather forecast by date from the repository.
    /// </summary>
    /// <param name="date">The date of the weather forecast to retrieve.</param>
    /// <returns>The weather forecast, or null if not found.</returns>
    Task<WeatherForecast?> GetWeatherForecast(string date);

    /// <summary>
    /// Deletes a weather forecast by date from the repository.
    /// </summary>
    /// <param name="date">The date of the weather forecast to delete.</param>
    Task DeleteWeatherForecast(string date);
}