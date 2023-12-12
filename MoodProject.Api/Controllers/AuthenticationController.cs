using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoodProject.Api.Configuration;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthenticationController
{
    private readonly AuthConfiguration AuthConfiguration;
    
    public AuthenticationController(AuthConfiguration authConfiguration)
    {
        AuthConfiguration = authConfiguration;
    }
    
    [HttpGet, ActionName("GetToken")]
    public Token GetToken(string userId)
    {
        var token = new JwtSecurityToken(
            AuthConfiguration.ValidIssuer,
            AuthConfiguration.ValidAudiences[0],
            new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userId),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
            },
            expires: DateTime.Now.AddHours(AuthConfiguration.HoursBeforeTokenExpiration),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Guid.NewGuid().ToByteArray()),
                SecurityAlgorithms.HmacSha256
            )
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new Token() { Value = tokenString };
    }
}