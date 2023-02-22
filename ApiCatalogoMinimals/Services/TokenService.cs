using ApiCatalogoMinimals.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCatalogoMinimals.Services;

public class TokenService : ITokenService
{
    public string GerarToken(string key, string issuer, string audience, UserModel user)
    {
        var claims = new[]
        {
           new Claim(ClaimTypes.Name, user.UserName),
           new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
       };

        var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));   

        var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims,
            expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);

        //de posse do token, vamos desserializar ele

        var tokenhandler = new JwtSecurityTokenHandler();
        var stringToken = tokenhandler.WriteToken(token);
        return stringToken;
    }
}
