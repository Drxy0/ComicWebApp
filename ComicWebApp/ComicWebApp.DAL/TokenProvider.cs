using ComicWebApp.DAL.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ComicWebApp.DAL;

public sealed class TokenProvider(IConfiguration configuration)
{
    public string Create(User user)
    {
        string secretKey = configuration["Jwt:SecretKey"]!;
        var securitKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        //var securityKey = configuration["Jwt:SecretKey"]u8.ToArray();

        var credentials = new SigningCredentials(securitKey, SecurityAlgorithms.HmacSha256);

        // QUESTION: Is there a difference between a good and a bad descriptor, what's its purpose??
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                // How do i choose which ones should go here??
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler(); // better than JwtSecurityTokenHandler

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }


    // QUESTIONS: Jel ovo ovako kako sam napisao dolje?
    // Access tokens are used to access the API, they should expire quickly, ~1h
    // Refresh tokens are used to get new Access tokens, they should expire after 7-30 days, stored in cookies
    // Best practice - create a new refresh token every time you create a new access token
    // i.e. use refresh token once
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

}
