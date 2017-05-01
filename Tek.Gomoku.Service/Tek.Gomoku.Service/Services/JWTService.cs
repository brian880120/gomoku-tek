using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public class JWTService : IJWTService
    {
        private readonly UserManager<Player> _userMgr;
        private readonly IConfigurationRoot _config;

        public JWTService(
            UserManager<Player> userMgr,
            IConfigurationRoot config)
        {
            _userMgr = userMgr;
            _config = config;
        }

        private async Task<Player> AddNewUser(string userName)
        {
            var player = new Player()
            {
                UserName = userName,
                Email = userName + "@teksystems.com"
            };

            var userResult = await _userMgr.CreateAsync(player, "P@ssw0rd!");
            var roleResult = await _userMgr.AddToRoleAsync(player, "Admin");
            var claimResult = await _userMgr.AddClaimAsync(player, new Claim("SuperUser", "True"));

            if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to add new user!");
            }

            return player;
        }

        public async Task<JwtSecurityToken> CreateToken(string userName)
        {
            var user = await _userMgr.FindByNameAsync(userName);
            if (user == null)
            {
                user = await AddNewUser(userName);
            }

            var userClaims = await _userMgr.GetClaimsAsync(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }.Union(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Tokens:Issuer"],
                audience: _config["Tokens:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            return token;
        }
    }
}
