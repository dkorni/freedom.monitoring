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

public class MonitorManagementService : IMonitorManagementService
{
    private readonly IServerInfoRepository _serverInfoRepository;
    private readonly IConfiguration _configuration;

    public MonitorManagementService(IServerInfoRepository serverInfoRepository, IConfiguration configuration)
    {
        _serverInfoRepository = serverInfoRepository;
        _configuration = configuration;
    }
    
    public Task<ServerInfoModel[]> GetAll()
    {
        return _serverInfoRepository.GetAllActive();
    }

    public async Task StayAlive(ServerInfoModel server)
    {
        var exist = await _serverInfoRepository.Get(server.Id);
        if (exist != null)
            await _serverInfoRepository.Update(exist);
    }
}