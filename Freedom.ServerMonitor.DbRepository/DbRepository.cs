using System.Data.Entity;
using AutoMapper;
using Freedom.ServerMonitor.DbRepository.Entities;
using Freedom.ServerMonitor.Domain.Enums;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;

namespace Freedom.ServerMonitor.DbRepository;

public class DbRepository : IServerInfoRepository
{
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly IMapper _mapper;

    public DbRepository(IKeyVaultManager keyVaultManager ,IMapper mapper)
    {
        _keyVaultManager = keyVaultManager;
        var configuration = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<ServerInfoModel, ServerInfoEntity>();
            cfg.CreateMap<ServerInfoEntity, ServerInfoModel>();
        });
        _mapper = configuration.CreateMapper();
    }
    
    public Task<ServerInfoModel[]> GetAll()
    {
        using var context = new DataBaseContext(_keyVaultManager);
        var entities = context.ServerInfos.ToArray();
        return Task.FromResult(entities.Select(x => _mapper.Map<ServerInfoModel>(x)).ToArray());
    }
    
    public Task<ServerInfoModel[]> GetAllActive()
    {
        using var context = new DataBaseContext(_keyVaultManager);
        var entities = context.ServerInfos.Where(x=>x.Status == ServerStatus.Active).ToArray();
        return Task.FromResult(entities.Select(x => _mapper.Map<ServerInfoModel>(x)).ToArray());
    }

    public async Task<ServerInfoModel> Get(Guid id)
    {
        using var context = new DataBaseContext(_keyVaultManager);
        var entity = await context.ServerInfos.FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<ServerInfoModel>(entity);
    }

    public async Task Create(ServerInfoModel serverInfo)
    {
        await using (   var context = new DataBaseContext(_keyVaultManager))
        {
            var entity = _mapper.Map<ServerInfoEntity>(serverInfo);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            context.ServerInfos.Add(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(ServerInfoModel serverInfoModel)
    { 
        await using (var context = new DataBaseContext(_keyVaultManager))
        {
            var server = await context.ServerInfos.FirstOrDefaultAsync(x => x.Id == serverInfoModel.Id);

            if (server is null)
            {
                server = await context.ServerInfos.FirstOrDefaultAsync(x => x.IpAddress == serverInfoModel.IpAddress && x.Name == serverInfoModel.Name);
            }

            if (server is null)
                throw new ArgumentNullException($"Can't find server with id {serverInfoModel.Id} or address {serverInfoModel.IpAddress}:{serverInfoModel.Port}");

            server.Name = serverInfoModel.Name;
            server.MaxPlayer = serverInfoModel.MaxPlayer;
            server.PlayerCount = serverInfoModel.PlayerCount;
            server.Status = serverInfoModel.Status;
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateBulk(ServerInfoModel[] servers)
    {
        await using (var context = new DataBaseContext(_keyVaultManager))
        {
            var entities = servers.Select(x => _mapper.Map<ServerInfoEntity>(x)).ToArray();
            context.ServerInfos.UpdateRange(entities);
            await context.SaveChangesAsync();
        }
    }
}