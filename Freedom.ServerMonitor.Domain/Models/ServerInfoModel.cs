using Freedom.ServerMonitor.Domain.Enums;

namespace Freedom.ServerMonitor.Domain.Models;

public class ServerInfoModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public int PlayerCount { get; set; }
    
    public int MaxPlayer { get; set; }
    
    public string IpAddress { get; set; }

    public int Port { get; set; }

    public ServerStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}