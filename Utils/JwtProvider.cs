using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using MessangerBack.Models;
using MessangerBack.Options;


namespace MessangerBack.Utils;

public class JwtProvider : IJwtProvider
{
    JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateTokenByUserId(string userId)
    {
        Claim[] claims = [ new(ClaimTypes.NameIdentifier, userId) ];

        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256
        );
        JwtSecurityToken token = new(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}