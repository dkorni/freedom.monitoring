namespace Freedom.ServerMonitor.Domain.Models;

//[Index(nameof(Name), nameof(IpAddress))]
public class ServerInfoModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public int PlayerCount { get; set; }
    
    public int MaxPlayer { get; set; }
    
    public string IpAddress { get; set; }

    public int Port { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}