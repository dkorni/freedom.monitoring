using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.Models;
using Microsoft.AspNetCore.Mvc;

namespace Freedom.ServerMonitor.Controllers;

[ApiController]
[Route("[controller]")]
public class ServerInfoController : ControllerBase
{
    private readonly IServerInfoRepository _serverInfoRepository;

    public ServerInfoController(IServerInfoRepository serverInfoRepository)
    {
        _serverInfoRepository = serverInfoRepository;
    }
    
    [HttpGet]
    public Task<ServerInfoModel[]> GetAll()
    {
       return _serverInfoRepository.GetAll();
    }

    [HttpPost]
    public Task Create(ServerInfoModel serverInfo)
    {
       return _serverInfoRepository.Create(serverInfo);
    }
}