using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        //At first add a security key but encode it (Hash it)
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
        }
        public string GenerateToken(AppUser appUser)
        {
            //Some data about user // put here data but not sensitive Dat
            var cliams = new List<Claim>
            {
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim(ClaimTypes.GivenName, appUser.DisplayName),
                new Claim("UserId", appUser.Id),
                new Claim("UserName", appUser.UserName)
            };
            // make some Credientials >> to validate the user token from this Credintials
            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256);

            // collect these variables into TokenDiscriptor
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(cliams),
                Issuer = _configuration["Token:Issuer"],
                IssuedAt= DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials= creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);
            
            return tokenHandler.WriteToken(token);

        }
    }
}
