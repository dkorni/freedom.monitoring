using AutoMapper;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;
using Freedom.ServerMonitor.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Freedom.ServerMonitor.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }
    
    [HttpPost]
    public Task<string> RegisterService(ServerInfoDto serverInfo)
    {
        var model = _mapper.Map<ServerInfoModel>(serverInfo);
        return _authService.RegisterServiceAsync(model);
    }
}