using Microsoft.Extensions.Caching.Memory;

namespace Graphene.Server;

internal class BackgroundHostedService : BackgroundService
{
    private readonly IMemoryCache memoryCache;

    public BackgroundHostedService(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000, stoppingToken);
        }
    }
}