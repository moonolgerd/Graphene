using Castle.Core.Logging;
using Graphene.Server.Models;
using Graphene.Server.Mutations;
using Graphene.Server.Queries;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Redis.OM;
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
    public async Task GetPeople_Test()
    {
        var authMock = Substitute.For<IAuthorizationService>();

        var authProviderMock = Substitute.For<IAuthorizationPolicyProvider>();

        var serviceCollection = new ServiceCollection();
        var provider = serviceCollection
            .BuildServiceProvider();

        var executor = await new ServiceCollection()
            .AddSingleton(new RedisConnectionProvider("redis://localhost:6379"))
            .AddSingleton(authMock)
            .AddSingleton(authProviderMock)
            .AddHostedService<IndexCreationService>()
            .AddGraphQL()
            //.AddAuthorization()
            .AddQueryType()
            .AddTypeExtension<PersonsQuery>()
            .BuildRequestExecutorAsync();

        var query = QueryRequestBuilder.New()
            .SetQuery("""
query GetPeople {
    people {
       id
       firstName
       lastName
       age
       personalStatement
       address {
           streetNumber
           streetName
           city
           state
           postalCode
           country
       }
    }
}
""")
            .AddGlobalState(WellKnownContextData.UserState, new UserState(new ClaimsPrincipal(new ClaimsIdentity()), true))
            .AddGlobalState("UserId", "Alice")
            .Create();

        var result = await executor.ExecuteAsync(query);

        result.MatchSnapshot();
    }

    [TestCase("Alice", "McGee", 17, "Main St", 4, "New York")]
    [TestCase("Alice", "Carroll", 10, "Broadway", 100, "London")]
    public async Task AddPerson_Test(string firstName, string lastName, int age, string streetName, int streetNumber, string city)
    {
        var authMock = Substitute.For<IAuthorizationService>();

        var authProviderMock = Substitute.For<IAuthorizationPolicyProvider>();

        var serviceCollection = new ServiceCollection();
        var provider = serviceCollection
            .BuildServiceProvider();

        var executor = await new ServiceCollection()
            .AddSingleton(new RedisConnectionProvider("redis://localhost:6379"))
            .AddSingleton(authMock)
            .AddSingleton(authProviderMock)
            .AddHostedService<IndexCreationService>()
            .AddGraphQL()
            //.AddAuthorization()
            .AddQueryType()
            .AddMutationType()
            .AddTypeExtension<PersonsQuery>()
            .AddTypeExtension<PersonsMutation>()
            .BuildRequestExecutorAsync();

        var query = QueryRequestBuilder.New()
            .SetQuery("""
mutation AddPerson($personInput: PersonInput!) {
    addPerson(personInput: $personInput) {
       id
       firstName
       lastName
       age
       personalStatement
       address {
           streetNumber
           streetName
           city
       }
    }
}
""")
            .AddVariableValue("personInput", new PersonInput(firstName, lastName, age)
            {
                StreetName = streetName,
                StreetNumber = streetNumber,
                City = city
            })
            .AddGlobalState(WellKnownContextData.UserState, new UserState(new ClaimsPrincipal(new ClaimsIdentity()), true))
            .AddGlobalState("UserId", "Alice")
            .Create();

        var result = await executor.ExecuteAsync(query);

        result.MatchSnapshot();
    }
}