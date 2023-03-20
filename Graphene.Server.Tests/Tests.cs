using Castle.Core.Logging;
using Graphene.Server.Models;
using Graphene.Server.Queries;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Snapshooter.NUnit;
using System.Security.Claims;

namespace Graphene.Server.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task WeatherForecast_Test()
    {
        var authMock = Substitute.For<IAuthorizationService>();

        var authProviderMock = Substitute.For<IAuthorizationPolicyProvider>();

        var loggerSub = Substitute.For<ILogger<WeatherForecastQuery>>();

        var executor = await new ServiceCollection()
            .AddSingleton(loggerSub)
            .AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>()
            .AddSingleton(authMock)
            .AddSingleton(authProviderMock)
            .AddGraphQL()
            .AddAuthorization()
            .AddQueryType()
            .AddTypeExtension<WeatherForecastQuery>()
            .BuildRequestExecutorAsync();

        var query = QueryRequestBuilder.New()
            .SetQuery("""
query WeatherForecast {
    weatherForecast {
        date
        temperatureC
        summary
    }
}
""")
            .AddGlobalState(WellKnownContextData.UserState, new UserState(new ClaimsPrincipal(new ClaimsIdentity()), true))
            .Create();
                
        var result = await executor.ExecuteAsync(query);

        result.MatchSnapshot();
    }

    [Test]
    public async Task Joke_Test()
    {
        // https://chillicream.com/blog/2019/04/11/integration-tests
        var cache = new MemoryCache(new MemoryCacheOptions
        {

        });

        var authMock = Substitute.For<IAuthorizationService>();

        var authProviderMock = Substitute.For<IAuthorizationPolicyProvider>();

        var serviceCollection = new ServiceCollection();
        var provider = serviceCollection
            .AddSingleton<JokeService>()
            .BuildServiceProvider();

        var executor = await new ServiceCollection()
            .AddSingleton<IMemoryCache>(cache)
            .AddSingleton(authMock)
            .AddSingleton(authProviderMock)
            .AddMemoryCache()
            .AddHostedService<BackgroundHostedService>()
            .AddGraphQL()
            .AddAuthorization()
            .AddQueryType()
            .AddTypeExtension<JokesQuery>()
            .BuildRequestExecutorAsync();

        var bs = provider.GetRequiredService<JokeService>();
        cache.Set("joke", bs.GetJoke());

        var query = QueryRequestBuilder.New()
            .SetQuery("""
query Joke {
    joke {
        setup
        punchline
    }
}
""")
            .AddGlobalState(WellKnownContextData.UserState, new UserState(new ClaimsPrincipal(new ClaimsIdentity()), true))
            .AddGlobalState("UserId", "Alice")
            .Create();

        var result = await executor.ExecuteAsync(query);

        result.MatchSnapshot();
    }
}