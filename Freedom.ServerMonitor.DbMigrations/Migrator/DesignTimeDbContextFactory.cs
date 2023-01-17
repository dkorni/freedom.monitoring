using System.Reflection;
using Freedom.ServerMonitor.DbRepository;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Logic.IoC;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freedom.ServerMonitor.DbMigrations.Migrator;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataBaseContext>
{
    public DataBaseContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddKeyVault(isDevelopment:true, configuration);
        var provider = serviceCollection.BuildServiceProvider();
        var keyVaultManager = provider.GetRequiredService<IKeyVaultManager>();
        
        return new DataBaseContext(keyVaultManager);
    }
}