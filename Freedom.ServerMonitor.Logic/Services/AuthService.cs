using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Freedom.ServerMonitor.Domain.Enums;
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Freedom.ServerMonitor.Logic.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IServerInfoRepository _serverInfoRepository;

    public AuthService(IConfiguration configuration, IServerInfoRepository serverInfoRepository)
    {
        _configuration = configuration;
        _serverInfoRepository = serverInfoRepository;
    }
    
    public async Task<string> RegisterServiceAsync(ServerInfoModel server)
    {
        server.Status = ServerStatus.Active;
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("ServerId", server.Id.ToString()),
            new Claim("Name", server.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: null,
            signingCredentials: signIn);

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        await _serverInfoRepository.Create(server);
        return tokenStr;
    }
}