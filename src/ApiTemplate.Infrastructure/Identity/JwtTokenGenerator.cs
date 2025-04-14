using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTemplate.Infrastructure.Identity;

/// <summary>
/// JWT settings from configuration
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// JWT secret key
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    
    /// <summary>
    /// JWT issuer
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
    
    /// <summary>
    /// JWT audience
    /// </summary>
    public string Audience { get; set; } = string.Empty;
    
    /// <summary>
    /// Token expiration time in minutes
    /// </summary>
    public int ExpiryMinutes { get; set; } = 60;
}

/// <summary>
/// Generates JWT tokens for authentication
/// </summary>
public class JwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    /// Creates a new instance of JwtTokenGenerator
    /// </summary>
    /// <param name="configuration">Application configuration</param>
    public JwtTokenGenerator(IConfiguration configuration)
    {
        _jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(_jwtSettings);
    }

    /// <summary>
    /// Generates a JWT token for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userName">User name</param>
    /// <param name="roles">User roles</param>
    /// <returns>JWT token</returns>
    public string GenerateToken(string userId, string userName, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add roles as claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}