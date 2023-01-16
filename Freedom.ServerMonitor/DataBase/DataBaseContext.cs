using System;
using System.Data.Entity;
using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Freedom.ServerMonitor.DataBase;

public class DataBaseContext : DbContext
{
    private readonly IKeyVaultManager _keyVault;
    public Microsoft.EntityFrameworkCore.DbSet<ServerInfoModel> ServerInfos { get; set; }

    public DataBaseContext(IKeyVaultManager keyVault)
    {
        _keyVault = keyVault;
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<ServerInfoModel>().HasData(new ServerInfoModel
    //         {
    //             Id = Guid.NewGuid(),
    //             Name = "Server only for true cossaks",
    //             PlayerCount = 24,
    //             MaxPlayer = 100,
    //             IpAddress = "128.0.0.1",
    //             Port = 7777
    //         },
    //         new ServerInfoModel
    //         {
    //             Id = Guid.NewGuid(),
    //             Name = "Bobber curva",
    //             PlayerCount = 3,
    //             MaxPlayer = 100,
    //             IpAddress = "128.2.2.8",
    //             Port = 1488
    //         }
    //         ,
    //         new ServerInfoModel
    //         {
    //             Id = Guid.NewGuid(),
    //             Name = "First Europe server",
    //             PlayerCount = 52,
    //             MaxPlayer = 100,
    //             IpAddress = "192.168.1.1",
    //             Port = 7777
    //         });
    //     base.OnModelCreating(modelBuilder);
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_keyVault.GetSecret("freedom--servermonitor--connection--string"));
    }
    
}