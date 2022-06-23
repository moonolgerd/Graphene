using Graphene.Server.Models;
using Graphene.Server.Mutations;
using Graphene.Server.Queries;
using Graphene.Server.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddSingleton<WeatherForecastRepository>();

services.AddGraphQLServer()
    .AddQueryType<WeatherForecastQuery>()
    .AddMutationType<WeatherForecastMutation>()
    .AddSubscriptionType<WeatherForecastSubscription>();

services.AddInMemorySubscriptions();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseWebSockets()
    .UseRouting();

app.UseCors("AllowAll");

app.MapGraphQL();

app.Run();