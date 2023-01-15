using System;
using Freedom.ServerMonitor.Enums;

namespace Freedom.ServerMonitor.Models;

public class ServerInfoModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public int PlayerCount { get; set; }
    
    public int MaxPlayer { get; set; }
    
    public string IpAddress { get; set; }

    public int Port { get; set; }
    
    public ServerStatus Status { get; set; }
}