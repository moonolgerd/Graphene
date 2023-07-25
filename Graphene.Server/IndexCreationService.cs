using Graphene.Server.Models;
using Redis.OM;
using Redis.OM.Contracts;
using System;

namespace Graphene.Server;

public class IndexCreationService : IHostedService
{
    private readonly IRedisConnectionProvider _provider;
    public IndexCreationService(IRedisConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _provider.Connection.CreateIndexAsync(typeof(Person));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
