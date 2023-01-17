using Freedom.ServerMonitor.DbRepository.Entities;
using Freedom.ServerMonitor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Freedom.ServerMonitor.DbRepository;

public class DataBaseContext : DbContext
{
    private readonly IKeyVaultManager _keyVault;
    public Microsoft.EntityFrameworkCore.DbSet<ServerInfoEntity> ServerInfos { get; set; }

    public DataBaseContext(IKeyVaultManager keyVault)
    {
        _keyVault = keyVault;
    }
    
    public DataBaseContext(IKeyVaultManager keyVault, DbContextOptions<DataBaseContext> options) : base(options)
    {
        _keyVault = keyVault;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_keyVault.GetSecret("freedom--servermonitor--connection--string"), b=>b.MigrationsAssembly("Freedom.ServerMonitor.DbMigrations"));
    }
}