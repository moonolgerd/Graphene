using Dapr.Actors.Client;
using Google.Protobuf.WellKnownTypes;
using Graphene.Server.Actors;
using Graphene.Server.Models;
using Graphene.Server.Mutations;
using Graphene.Server.Queries;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
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

        var actorFactory = Substitute.For<IActorProxyFactory>();
        var weatherActor = Substitute.For<IWeatherForecastActor>();
        actorFactory
            .CreateActorProxy<IWeatherForecastActor>(new Dapr.Actors.ActorId("WeatherForecastActor"), nameof(WeatherForecastActor))
            .Returns(weatherActor);

        weatherActor.GetWeatherForecastsAsync(5).Returns(new List<WeatherForecast>
        {
            new("01/01/2023", 48, "Scorching"),
            new("01/02/2023", 19, "Chilly"),
            new("01/03/2023", -3, "Sunny"),
            new("01/04/2023", 48, "Sunny"),
            new("01/05/2023", 31, "Scorching")
        });

        var executor = await new ServiceCollection()
            .AddSingleton(loggerSub)
            .AddSingleton(authMock)
            .AddSingleton(authProviderMock)
            .AddSingleton(actorFactory)
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

        var personRepositoryMock = Substitute.For<IPersonRepository>();

        personRepositoryMock.GetAllPeople().Returns(new List<Person>
        {
            new Person
            {
                FirstName = "Alice",
                LastName = "McGee",
                Age = 17,
                Address = new Address {
                    StreetNumber = 4,
                    StreetName = "Main St",
                    City = "New York"
                }
            }
        });

        var serviceCollection = new ServiceCollection();
        var provider = serviceCollection
            .BuildServiceProvider();

        var executor = await new ServiceCollection()
            .AddSingleton(personRepositoryMock)
            .AddSingleton(authMock)
            .AddSingleton(authProviderMock)
            .AddHostedService<IndexCreationService>()
            .AddGraphQL()
            .AddQueryType()
            .AddTypeExtension<PersonQuery>()
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

        var actorFactory = Substitute.For<IActorProxyFactory>();
        var personActor = Substitute.For<IPersonActor>();
        actorFactory
            .CreateActorProxy<IPersonActor>(new Dapr.Actors.ActorId("PersonActor"), nameof(PersonActor))
            .Returns(personActor);

        var serviceCollection = new ServiceCollection();
        var provider = serviceCollection
            .BuildServiceProvider();

        var executor = await new ServiceCollection()
            .AddSingleton(actorFactory)
            .AddSingleton(authMock)
            .AddSingleton(authProviderMock)
            .AddHostedService<IndexCreationService>()
            .AddGraphQL()
            .AddQueryType()
            .AddMutationType()
            .AddTypeExtension<PersonQuery>()
            .AddTypeExtension<PersonMutation>()
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