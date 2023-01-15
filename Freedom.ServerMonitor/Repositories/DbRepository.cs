using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.DataBase;
using Freedom.ServerMonitor.Models;

namespace Freedom.ServerMonitor.Repositories;

public class DbRepository : IServerInfoRepository
{
    private readonly DataBaseContext _context;

    public DbRepository(DataBaseContext context)
    {
        _context = context;
    }
    
    public Task<ServerInfoModel[]> GetAll()
    {
        return Task.Run(() => _context.ServerInfos.ToArray());
    }

    public Task Create(ServerInfoModel serverInfo)
    {
        _context.Add(serverInfo);
        return _context.SaveChangesAsync();
    }
}