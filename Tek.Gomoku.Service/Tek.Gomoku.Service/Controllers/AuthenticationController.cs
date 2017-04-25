using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tek.Gomoku.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tek.Gomoku.Service.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tek.Gomoku.Service.Controllers
{
    [Produces("application/json")]
    public class AuthenticationController : Controller
    {
        private ILogger<AuthenticationController> _logger;
        private UserManager<Player> _userMgr;
        private IPasswordHasher<Player> _hasher;
        private IConfigurationRoot _config;
        private readonly IUserInfoService _userInfoService;
        private readonly ISocketService _socketService;
        private readonly GameContext _context;

        public AuthenticationController(
            UserManager<Player> userMgr,
            IPasswordHasher<Player> hasher,
            ILogger<AuthenticationController> logger,
            IConfigurationRoot config,
            IUserInfoService userInfoService,
            ISocketService socketService,
            GameContext context)
        {
            _logger = logger;
            _userMgr = userMgr;
            _hasher = hasher;
            _config = config;
            _userInfoService = userInfoService;
            _socketService = socketService;
            _context = context;
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

        private async Task<JwtSecurityToken> CreateToken(string userName)
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
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return token;
        }

        private async Task<Game> JoinGame(string userName, string side)
        {
            var game = _context.Game.FirstOrDefault();
            if (game == null)
            {
                game = new Game();
                await _context.Game.AddAsync(game);
            }
            else if (!string.IsNullOrWhiteSpace(game.BlackSidePlayer) && !string.IsNullOrWhiteSpace(game.WhiteSidePlayer))
            {
                throw new InvalidOperationException("Game has started!");
            }

            if (!string.IsNullOrWhiteSpace(game.BlackSidePlayer) && side == "black")
            {
                throw new InvalidOperationException("Black side has been taken!");
            }

            if (!string.IsNullOrWhiteSpace(game.WhiteSidePlayer) && side == "white")
            {
                throw new InvalidOperationException("White side has been taken!");
            }

            if(side == "black")
            {
                game.BlackSidePlayer = userName;
            }

            if(side == "white")
            {
                game.WhiteSidePlayer = userName;
            }

            await _context.SaveChangesAsync();

            return game;
        }

        private async Task<Game> QuitGame(string userName)
        {
            var game = _context.Game.FirstOrDefault();
            if (game == null)
            {
                throw new InvalidOperationException("Game not started yet!");
            }
            else if (game.BlackSidePlayer != userName && game.WhiteSidePlayer != userName)
            {
                throw new InvalidOperationException($"{userName} is not part of the game!");
            }

            if (game.BlackSidePlayer == userName)
            {
                game.BlackSidePlayer = null;
            }
            else
            {
                game.WhiteSidePlayer = null;
            }

            await _context.SaveChangesAsync();

            return game;
        }

        private async Task BroadcastGameInfo(Game game)
        {
            var jsonString = JsonConvert.SerializeObject(game, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            await _socketService.BroadcastMessage(jsonString);
        }

        [HttpPost("api/authentication/token")]
        public async Task<IActionResult> SignIn([FromBody] CredentialModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Side))
                {
                    throw new InvalidOperationException("UserName and Side are required parameters");
                }

                var game = await JoinGame(model.UserName, model.Side);

                await BroadcastGameInfo(game);

                var token = await CreateToken(model.UserName);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return BadRequest("Failed to generate token");
        }

        [HttpDelete("api/authentication/token")]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                var userName = _userInfoService.GetUserName(User);

                if (string.IsNullOrWhiteSpace(userName))
                {
                    throw new InvalidOperationException("You haven't signed in yet!");
                }

                var game = await QuitGame(userName);

                await BroadcastGameInfo(game);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while signing out: {ex}");
            }

            return BadRequest("Failed to sign out");
        }
    }
}