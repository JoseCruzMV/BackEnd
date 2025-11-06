using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Models.Token;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    public JwtSettings _jwtSetting { get; }
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor,
        IOptions<JwtSettings> jwtSettings)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtSetting = jwtSettings.Value;
    }

    public string CreateToken(Usuario usuario, IList<string>? roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, usuario.UserName!),
            new("userId", usuario.Id),
            new("email", usuario.Email!)
        };

        foreach (var role in roles!)
        {
            var claim = new Claim(ClaimTypes.Role, role);
            claims.Add(claim);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key!)));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtSetting.SpareTime),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token); 
    }

    public string GetSessionUser()
    {
        var username = _httpContextAccessor.HttpContext!.User?.Claims?
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
            .Value;
        return username??"";
    }
}