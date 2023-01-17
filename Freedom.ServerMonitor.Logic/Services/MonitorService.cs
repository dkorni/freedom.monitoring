using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Freedom.ServerMonitor.Logic.Services;

public class MonitorService : IMonitorService, IDisposable
{
    private readonly IServerInfoRepository _serverInfoRepository;
    private readonly IConfiguration _configuration;
    private CancellationTokenSource _monitorCancelationTokenSource;
    private int _maxLiveInterval;
    private int _monitorLoopDelay;
    private object _lockObject = new();
    private readonly SemaphoreSlim _lock= new SemaphoreSlim(1, 1);
    
    public MonitorService(IServerInfoRepository serverInfoRepository, IConfiguration configuration)
    {
        _serverInfoRepository = serverInfoRepository;
        _configuration = configuration;
        _maxLiveInterval = int.Parse(configuration["MaxLiveInterval"]);
        _monitorLoopDelay = int.Parse(configuration["MonitorLoopDelay"]);
        _monitorCancelationTokenSource = new CancellationTokenSource();
        // _ = MonitorLoop(_monitorCancelationTokenSource.Token);
    }
    
    public Task<ServerInfoModel[]> GetAll()
    {
        return _serverInfoRepository.GetAll();
    }

    public async Task<string> CreateServiceAsync(ServerInfoModel server)
    {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("ServerId", server.Id.ToString()),
            new Claim("Name", server.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: null,
            signingCredentials: signIn);

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        await _serverInfoRepository.Create(server);
        return tokenStr;
    }
    
    public async Task StayAlive(ServerInfoModel server)
    {
        await _lock.WaitAsync();
        try
        {
            var exist = await _serverInfoRepository.Get(server.Name, server.IpAddress, server.Port);
            if (exist != null)
                await _serverInfoRepository.Update(exist);
            else
            {
                await _serverInfoRepository.Create(exist);
            }
        }
        catch
        {
            _lock.Release();
        }
    }

    private async Task MonitorLoop(CancellationToken cancellationToken)
    {
        Log.Information("Starting monitoring loopp...");
        while (!cancellationToken.IsCancellationRequested)
        {
            await _lock.WaitAsync();
            try
            {
                var servers = await _serverInfoRepository.GetAll();
                var aliveServers = new List<ServerInfoModel>();
                foreach (var server in servers)
                {
                    var diff = DateTime.UtcNow - server.UpdatedAt;
                    if(diff.Seconds < _maxLiveInterval)
                        aliveServers.Add(server);
                }

                await _serverInfoRepository.UpdateBulk(aliveServers.ToArray());
               
            }
            finally
            {
                _lock.Release();
            }
            await Task.Delay(_monitorLoopDelay);
        }
    }
    
    public void Dispose()
    {
        Log.Information("Stopping monitoring...");
        _monitorCancelationTokenSource.Cancel();
    }
}