using AutoMapper;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;
using Freedom.ServerMonitor.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freedom.ServerMonitor.Controllers;

[ApiController]
[Route("[controller]")]
public class ServerInfoController : ControllerBase
{
    private readonly IServerInfoRepository _serverInfoRepository;
    private readonly IMonitorManagementService _monitorService;
    private readonly IMapper _mapper;

    public ServerInfoController(IMonitorManagementService monitorService, IMapper mapper)
    {
        _monitorService = monitorService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ServerInfoDto[]> GetAll()
    {
       var models = await _monitorService.GetAll();
       return models.Select(x => _mapper.Map<ServerInfoDto>(x)).ToArray();
    }

    [HttpPut]
    [Authorize]
    public Task StayAlive(ServerInfoModel serverInfo)
    {
        var model = _mapper.Map<ServerInfoModel>(serverInfo);
        return _monitorService.StayAlive(model);
    }
}