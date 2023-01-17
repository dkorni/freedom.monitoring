using Freedom.ServerMonitor.Domain.Enums;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Freedom.ServerMonitor.Logic.Services;

public class ServerWatcher : IServerWatcher, IDisposable
{
    private readonly IServerInfoRepository _serverInfoRepository;
    private int _maxLiveInterval;
    private int _monitorLoopDelay;
    private CancellationTokenSource _monitorCancelationTokenSource;
    
    public ServerWatcher(IServerInfoRepository serverInfoRepository, IConfiguration configuration)
    {
        _serverInfoRepository = serverInfoRepository;
        _maxLiveInterval = int.Parse(configuration["MaxLiveInterval"]);
        _monitorLoopDelay = int.Parse(configuration["MonitorLoopDelay"]);
        _monitorCancelationTokenSource = new CancellationTokenSource();
    }

    public void Run() => MonitorLoop(_monitorCancelationTokenSource.Token);

    private async Task MonitorLoop(CancellationToken cancellationToken)
    {
        Log.Information("Starting monitoring loop...");
        while (!cancellationToken.IsCancellationRequested)
        {
            var servers = await _serverInfoRepository.GetAll();
            var deadServers = new List<ServerInfoModel>();
            foreach (var server in servers)
            {
                if(server.Status == ServerStatus.Unknown)
                    continue;
                
                var diff = DateTime.UtcNow - server.UpdatedAt;
                if (diff > TimeSpan.FromSeconds(_maxLiveInterval))
                {
                    deadServers.Add(server);
                    server.Status = ServerStatus.Unknown;
                }
            }

            if(deadServers.Count > 0)
                await _serverInfoRepository.UpdateBulk(deadServers.ToArray());
            
            await Task.Delay(_monitorLoopDelay);
        }
    }
    
    public void Dispose()
    {
        Log.Information("Stopping watching...");
        _monitorCancelationTokenSource.Cancel();
    }
}