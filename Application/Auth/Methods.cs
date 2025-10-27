using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Auth;

public class Methods
{
    public static string CreateAccessToken(IConfiguration configuration,PersonEntity person)
    {
        var claim = new List<Claim>()
        {
            new Claim("fin",person.Fin),
            new Claim(ClaimTypes.NameIdentifier,person.PasswordHash),
            new Claim(ClaimTypes.Role,person.Role),
        };
            
        var token = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(token, SecurityAlgorithms.HmacSha512);
        var tokenDescription = new JwtSecurityToken(
            
            issuer:configuration["Jwt:Issuer"],
            audience:configuration["Jwt:audience"],
            claims:claim,
            signingCredentials:creds,
            expires:DateTime.UtcNow.AddMinutes(100));
            
        return new JwtSecurityTokenHandler().WriteToken(tokenDescription);
    }


    public static string CreateRefreshToken()
    {
        var num = new byte[32];
        var randomNum = RandomNumberGenerator.Create();
        randomNum.GetBytes(num);
        return Convert.ToBase64String(num);
    }
}