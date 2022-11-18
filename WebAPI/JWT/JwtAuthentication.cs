using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.JWT
{
    internal static class JwtAuthentication
    {
        private static readonly JwtSecurityTokenHandler JwtHandler = new JwtSecurityTokenHandler();
        private static string GenerateAccessToken(IEnumerable<Claim> claims = null)
        {
            var token = new JwtSecurityToken(issuer: JwtConstants.AccessConstants.Issuer,
                audience: JwtConstants.AccessConstants.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(JwtConstants.AccessConstants.ExpiryPeriodInMinutes),
                signingCredentials: new SigningCredentials(JwtConstants.AccessConstants.SecurityKey,
                    JwtConstants.AccessConstants.EncryptionAlgorithm)
            );
            return JwtHandler.WriteToken(token);
        }
        private static string GenerateRefreshToken(IEnumerable<Claim> claims = null)
        {
            var token = new JwtSecurityToken(issuer: JwtConstants.RefreshConstants.Issuer,
                audience: JwtConstants.RefreshConstants.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(JwtConstants.RefreshConstants.ExpiryPeriodInMinutes),
                signingCredentials: new SigningCredentials(JwtConstants.RefreshConstants.SecurityKey,
                    JwtConstants.RefreshConstants.EncryptionAlgorithm)
            );
            return JwtHandler.WriteToken(token);
        }
        public static TokenModel GetToken(ulong userId, string timeZoneOffset, string role)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(Claims.UserId.ToString(), userId.ToString()),
                    new Claim(Claims.Role.ToString(), role),
                    new Claim(Claims.TimeZoneOffset.ToString(), timeZoneOffset),
                };

                return new TokenModel(GenerateAccessToken(claims), GenerateRefreshToken(claims));
            }
            catch
            {
                throw;
            }
        }
        public static TokenModel GetToken(string refreshToken)
        {
            try
            {
                var token = JwtHandler.ValidateToken(refreshToken, JwtValidationParameters.RefreshTokenValidationParameters
                    , out SecurityToken securityToken);
                var userId = token.FindFirst(Claims.UserId.ToString()).Value;
                var role = token.FindFirst(Claims.Role.ToString()).Value;
                var timeZoneOffset = token.FindFirst(Claims.TimeZoneOffset.ToString()).Value;
                return GetToken(ulong.Parse(userId), timeZoneOffset, role);
            }
            catch
            {
                throw;
            }
        }
    }
}
