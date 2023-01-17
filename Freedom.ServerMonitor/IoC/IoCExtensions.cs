using System.Text;
using Freedom.ServerMonitor.DbRepository.IoC;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Extensions;
using Freedom.ServerMonitor.Logic.IoC;
using Freedom.ServerMonitor.Logic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Freedom.ServerMonitoring.RedisRepository.IoC;

namespace Freedom.ServerMonitor.IoC;

public static class IoCExtensions
{
    public static WebApplicationBuilder AddHostDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.AddKeyVault();
        builder.AddLogger();
        builder.Services.AddDbRepository();
        builder.Services.AddDbRepository();
        builder.Services.AddRedisCacheRepository();
        
        builder.Services.AddScoped<IMonitorService, MonitorService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMapper();
        
        return builder;
    }
}