using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ToDo.Domain.Keys;
using ToDo.Domain.Entities;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ToDo.Application.Services;

public class TokenService
{
    public static string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(ToDo.Domain.Keys.Key.Secret);
        var tokenConfig = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.AdminPrivileges.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public static long GetUserIdByToken(string token_recevied)
    {
        var token = token_recevied.Replace("Bearer", "").Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var id = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;

        var longId = long.Parse(id);
        
        return longId;

    }


}
    
