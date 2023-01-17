using Freedom.ServerMonitor.Domain.Models;

namespace Freedom.ServerMonitor.Domain.Interfaces;

public interface IAuthService
{
    Task<string> RegisterServiceAsync(ServerInfoModel server);
}