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
using Microsoft.EntityFrameworkCore;

namespace Tek.Gomoku.Service.Controllers
{
    [Produces("application/json")]
    public class AuthenticationController : Controller
    {
        private ILogger<AuthenticationController> _logger;
        private readonly IUserInfoService _userInfoService;
        private readonly IGameService _gameSerivce;
        private readonly IJWTService _jwtService;
        private readonly IConfigurationRoot _config;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IUserInfoService userInfoService,
            IGameService gameService,
            IJWTService jwtService,
            IConfigurationRoot config)
        {
            _logger = logger;
            _userInfoService = userInfoService;
            _gameSerivce = gameService;
            _jwtService = jwtService;
            _config = config;
        }

        [HttpPost("api/authentication/token")]
        public async Task<IActionResult> SignIn([FromBody] CredentialModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    throw new InvalidOperationException("UserName is required!");
                }

                var token = await _jwtService.CreateToken(model.UserName);

                await _gameSerivce.SignIn(model.UserName);

                if (_config["AutoPlay:Mode"] == "true")
                {
                    await _gameSerivce.SignIn("Machine");
                }

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

                await _gameSerivce.SignOut(userName);

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