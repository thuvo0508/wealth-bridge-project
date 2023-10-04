using AutoMapper;
using BorrowerProject.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WealthBridge.Core.Database;
using WealthBridge.Core.Models;
using WealthBridge.Core.Services.Interface;

namespace WealthBridge.Core.Services
{
    public class AuthService : IAuthService
    {
        private WealthBridgeDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;

        public AuthService(
            WealthBridgeDbContext context,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _configuration = configuration;
        }

        public Borrower FindByEmail(string email)
        {
            var user = _context.Borrower.SingleOrDefault(x => x.Email == email);

            // return null if user not found
            if (user == null) return null;

            return user;
        }

        public void Create(Borrower model)
        {
            // validate
            if (_context.Borrower.Any(x => x.NameFirst == model.Email))
                throw new Exception("User with the email '" + model.Email + "' already exists");

            // map model to new user object
            var user = _mapper.Map<Borrower>(model);

            // save user
            _context.Borrower.Add(user);
            _context.SaveChanges();
        }

        public void Update(Borrower model)
        {
            _context.Borrower.Update(model);
            _context.SaveChanges();
        }

        public AuthenticateResponse CreateUserAuthTokenAsync(Borrower user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim("id", user.IdBorrower.ToString())
                };

            // authentication successful so generate jwt token
            var token = CreateToken(authClaims);
            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            Update(user);

            return new AuthenticateResponse(token, refreshToken);
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
