using System.Threading.Tasks;
using AutoMapper;
using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.DTO;
using Freedom.ServerMonitor.Models;
using Microsoft.AspNetCore.Mvc;

namespace Freedom.ServerMonitor.Controllers;

[ApiController]
[Route("[controller]")]
public class ServerInfoController : ControllerBase
{
    private readonly IServerInfoRepository _serverInfoRepository;
    private readonly IMapper _mapper;

    public ServerInfoController(IServerInfoRepository serverInfoRepository, IMapper mapper)
    {
        _serverInfoRepository = serverInfoRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ServerInfoDto[]> GetAll()
    {
       var models = await _serverInfoRepository.GetAll();
       return models.Select(x => _mapper.Map<ServerInfoDto>(x)).ToArray();
    }

    [HttpPost]
    public Task Create(ServerInfoDto serverInfo)
    {
        var model = _mapper.Map<ServerInfoModel>(serverInfo);
        return _serverInfoRepository.Create(model);
    }

    [HttpPut]
    public Task Update(ServerInfoModel serverInfo)
    {
        var model = _mapper.Map<ServerInfoModel>(serverInfo);
        return _serverInfoRepository.Update(model);
    }
}