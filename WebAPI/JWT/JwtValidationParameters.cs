using Microsoft.IdentityModel.Tokens;
using System;

namespace WebAPI.JWT
{
    internal static class JwtValidationParameters
    {
        internal static TokenValidationParameters AccessTokenValidationParameters => new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = JwtConstants.AccessConstants.Issuer,
            ValidAudience = JwtConstants.AccessConstants.Audience,
            IssuerSigningKey = JwtConstants.AccessConstants.SecurityKey,
            ClockSkew = TimeSpan.Zero
        };
        internal static TokenValidationParameters RefreshTokenValidationParameters => new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = JwtConstants.RefreshConstants.Issuer,
            ValidAudience = JwtConstants.RefreshConstants.Audience,
            IssuerSigningKey = JwtConstants.RefreshConstants.SecurityKey,
            ClockSkew = TimeSpan.Zero
        };
    }
}
