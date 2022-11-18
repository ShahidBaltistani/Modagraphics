using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.JWT
{
    internal static class JwtConstants
    {
        static JwtConstants()
        {
            var audience = "Everyone";
            AccessConstants = new JwtConstantsStrucutre()
            {
                Secret = "Access token secret key." + audience,
                Audience = audience,
                EncryptionAlgorithm = SecurityAlgorithms.HmacSha256,
                ExpiryPeriodInMinutes = 10,
                Issuer = "VU360 Solutions"
            };

            RefreshConstants = new JwtConstantsStrucutre()
            {
                Secret = "Refresh token secret key here." + audience,
                Audience = audience,
                EncryptionAlgorithm = SecurityAlgorithms.HmacSha256,
                ExpiryPeriodInMinutes = 30,
                Issuer = "VU360 Solutions"
            };
        }

        internal static JwtConstantsStrucutre AccessConstants { get; }
        internal static JwtConstantsStrucutre RefreshConstants { get; }
    }

    internal struct JwtConstantsStrucutre
    {
        internal string Issuer { get; set; }
        internal string Audience { get; set; }
        internal string Secret { get; set; }
        internal int ExpiryPeriodInMinutes { get; set; }
        internal string EncryptionAlgorithm { get; set; }
        internal SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }

    public class UserClaims
    {
        public uint UserId { get; set; }
        public int TimeZoneOffset { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsActive { get; set; }
    }

    public enum Claims
    {
        UserId,
        TimeZoneOffset,
        Role
    }
}
