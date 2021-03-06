using Graphene.Server;
using Graphene.Server.Models;
using Graphene.Server.Mutations;
using Graphene.Server.Queries;
using Graphene.Server.Subscriptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Okta.AspNetCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddSingleton<WeatherForecastRepository>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
})
    .AddOktaWebApi(new OktaWebApiOptions()
    {
        OktaDomain = builder.Configuration["Authentication:Okta:OktaDomain"],
        AuthorizationServerId = "default",
        Audience = "api://default"
    });


services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.RequireClaim(ClaimTypes.Name);
    });
});

services.AddGraphQLServer()
    .AddHttpRequestInterceptor<HttpRequestInterceptor>()
    .AddAuthorization()
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

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();