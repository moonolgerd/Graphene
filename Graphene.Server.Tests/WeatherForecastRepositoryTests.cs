using NSubstitute;
using Redis.OM;
using Graphene.Server.Models;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Graphene.Server.Tests;

[TestFixture]
public class WeatherForecastRepositoryTests
{
    private IWeatherForecastRepository _repository;
    private IRedisConnectionProvider _connectionProviderMock;
    private IRedisCollection<WeatherForecast> _redisCollectionMock;

    [SetUp]
    public void SetUp()
    {
        _connectionProviderMock = Substitute.For<IRedisConnectionProvider>();
        _redisCollectionMock = Substitute.For<IRedisCollection<WeatherForecast>>();
        _connectionProviderMock.RedisCollection<WeatherForecast>().Returns(_redisCollectionMock);
        _repository = new WeatherForecastRepository(_connectionProviderMock);
    }

    [Test]
    public async Task GetWeatherForecasts_Should_Return_WeatherForecasts()
    {
        string[] summaries = new[]
  {
"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

        var expected = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index).ToString(),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToList();

        _redisCollectionMock.ToListAsync().Returns(expected);

        var result = await _repository.GetWeatherForecasts();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetWeatherForecast_Should_Return_WeatherForecast()
    {
        var expected = new WeatherForecast("2022-12-24", -5, "Chilly");
        _redisCollectionMock.FindByIdAsync(expected.Date).Returns(expected);

        var result = await _repository.GetWeatherForecast(expected.Date);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task AddWeatherForecast_Should_Add_WeatherForecast()
    {
        var input = new WeatherForecastInput("2022-12-24", -5, "Chilly");
        await _repository.AddWeatherForecast(input);

        await _redisCollectionMock.Received()
            .InsertAsync(Arg.Is<WeatherForecast>(
                w => w.Date == input.Date &&
                     w.TemperatureC == input.TemperatureC &&
                     w.Summary == input.Summary));
    }

    [Test]
    public async Task UpdateWeatherForecast_Should_Update_WeatherForecast()
    {
        var input = new WeatherForecastInput("2022-12-24", -5, "Chilly");
        await _repository.UpdateWeatherForecast(input);

        await _redisCollectionMock.Received()
            .UpdateAsync(Arg.Is<WeatherForecast>(
                w => w.Date == input.Date &&
                     w.TemperatureC == input.TemperatureC &&
                     w.Summary == input.Summary));
    }

    [Test]
    public async Task DeleteWeatherForecast_Should_Delete_WeatherForecast()
    {
        var date = "2022-12-24";
        var expected = new WeatherForecast(date, -5, "Chilly");
        _redisCollectionMock.FindByIdAsync(date).Returns(expected);

        await _repository.DeleteWeatherForecast(date);

        await _redisCollectionMock.Received().DeleteAsync(expected);
    }
}