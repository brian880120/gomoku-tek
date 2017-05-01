using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tek.Gomoku.Service.Models;
using Tek.Gomoku.Service.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Tek.Gomoku.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/GameMoves")]
    public class GameMovesController : Controller
    {
        private readonly ILogger<GameMovesController> _logger;
        private readonly GameContext _context;
        private readonly IUserInfoService _userInfoService;
        private readonly IGameService _gameService;

        public GameMovesController(
            ILogger<GameMovesController> logger,
            GameContext context, 
            IUserInfoService userInfoService,
            IGameService gameService)
        {
            _logger = logger;
            _context = context;
            _userInfoService = userInfoService;
            _gameService = gameService;
        }

        // GET: api/GameMoves
        [HttpGet]
        public IEnumerable<GameMove> GetGameMove()
        {
            return _context.GameMove;
        }

        // GET: api/GameMoves/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameMove([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gameMove = await _context.GameMove.SingleOrDefaultAsync(m => m.ID == id);

            if (gameMove == null)
            {
                return NotFound();
            }

            return Ok(gameMove);
        }

        // PUT: api/GameMoves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameMove([FromRoute] int id, [FromBody] GameMove gameMove)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gameMove.ID)
            {
                return BadRequest();
            }

            _context.Entry(gameMove).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameMoveExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GameMoves
        [HttpPost]
        public async Task<IActionResult> PostGameMove([FromBody] GameMove gameMove)
        {
            try
            {
                var userName = _userInfoService.GetUserName(User);
                if (string.IsNullOrWhiteSpace(userName))
                {
                    throw new InvalidOperationException("User name is required!");
                }

                await _gameService.Move(userName, gameMove);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Fail to move");
        }

        // DELETE: api/GameMoves
        [HttpDelete()]
        public async Task<IActionResult> DeleteGameMove()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.GameMove.Any())
            {
                return NotFound();
            }

            await _context.GameMove.ForEachAsync(p => _context.GameMove.Remove(p));
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool GameMoveExists(int id)
        {
            return _context.GameMove.Any(e => e.ID == id);
        }
    }
}