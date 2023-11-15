using Graphene.Server;
using Graphene.Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Okta.AspNetCore;
using Redis.OM;
using Redis.OM.Contracts;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddMemoryCache();

services.AddSingleton<IRedisConnectionProvider>(new RedisConnectionProvider(builder.Configuration["ConnectionStrings:Redis"]!));
services.AddHostedService<IndexCreationService>();
services.AddSingleton<JokeService>();
services.AddHostedService<BackgroundHostedService>();
services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();
services.AddSingleton<IPersonRepository, PersonRepository>();

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


services.AddAuthorizationBuilder()
    .AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.RequireClaim(ClaimTypes.Name);
    });

services.AddGraphQLServer()
    .AddHttpRequestInterceptor<HttpRequestInterceptor>()
    .AddAuthorization()
    .AddTypes()
    .AddInMemorySubscriptions();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

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