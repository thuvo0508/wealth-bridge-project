using System;
using System.IdentityModel.Tokens.Jwt;
using WealthBridge.Core.Database;

namespace WealthBridge.Core.Models
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }


        public AuthenticateResponse(JwtSecurityToken token, string refreshToken)
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token);
            RefreshToken = refreshToken;
            Expiration = token.ValidTo;
        }
    }
}
