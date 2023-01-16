using Freedom.ServerMonitor.Enums;

namespace Freedom.ServerMonitor.DTO;

public class ServerInfoDto
{
    public string Name { get; set; }

    public int PlayerCount { get; set; }
    
    public int MaxPlayer { get; set; }
    
    public string IpAddress { get; set; }

    public int Port { get; set; }
    
    public ServerStatus Status { get; set; }
}