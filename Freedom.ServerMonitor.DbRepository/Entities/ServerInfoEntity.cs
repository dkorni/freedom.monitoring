using Freedom.ServerMonitor.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Freedom.ServerMonitor.DbRepository.Entities;

[Index(nameof(Name), nameof(IpAddress))]
public class ServerInfoEntity
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