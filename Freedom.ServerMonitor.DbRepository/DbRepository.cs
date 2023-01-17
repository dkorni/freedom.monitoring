using System.Data.Entity;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;

namespace Freedom.ServerMonitor.DbRepository;

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

    public Task<ServerInfoModel> Get(string name, string address, int port)
    {
        throw new NotImplementedException();
    }

    public Task Create(ServerInfoModel serverInfo)
    {
        _context.Add(serverInfo);
        return _context.SaveChangesAsync();
    }

    public async Task Update(ServerInfoModel serverInfoModel)
    {
        var server = await _context.ServerInfos.FirstOrDefaultAsync(x => x.Id == serverInfoModel.Id);

        if (server is null)
        {
            server = await _context.ServerInfos.FirstOrDefaultAsync(x => x.IpAddress == serverInfoModel.IpAddress && x.Name == serverInfoModel.Name);
        }

        if (server is null)
            throw new ArgumentNullException($"Can't find server with id {serverInfoModel.Id} or address {serverInfoModel.IpAddress}:{serverInfoModel.Port}");

        server.Name = serverInfoModel.Name;
        server.MaxPlayer = serverInfoModel.MaxPlayer;
        server.PlayerCount = serverInfoModel.PlayerCount;
       // server.Status = serverInfoModel.Status;
    }

    public Task UpdateBulk(ServerInfoModel[] serverInfoModel)
    {
        throw new NotImplementedException();
    }
}