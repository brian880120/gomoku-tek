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

namespace Tek.Gomoku.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/GameMoves")]
    public class GameMovesController : Controller
    {
        private readonly GameContext _context;
        private readonly ISocketService _socketService;
        private readonly IUserInfoService _userInfoService;

        public GameMovesController(GameContext context, ISocketService socketService, IUserInfoService userInfoService)
        {
            _context = context;
            _socketService = socketService;
            _userInfoService = userInfoService;
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
            var userName = _userInfoService.GetUserName(User);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GameMove.Add(gameMove);

            var game = _context.Game.FirstOrDefault();
            if (game == null || string.IsNullOrWhiteSpace(game.BlackSidePlayer) && string.IsNullOrWhiteSpace(game.WhiteSidePlayer))
            {
                return BadRequest("Game not started yet!");
            }

            if (game.NextPlayer != userName)
            {
                return BadRequest("!It's not your turn");
            }

            var whitePlayer = game.WhiteSidePlayer;
            var blackPlayer = game.BlackSidePlayer;
            var currentPlayer = userName;
            var nextPlayer = currentPlayer == whitePlayer ? blackPlayer : whitePlayer;
            game.NextPlayer = nextPlayer;

            var colorInString = userName == game.BlackSidePlayer ? "black" : "white";
            gameMove.ColorInString = colorInString;

            await _context.SaveChangesAsync();

            var jsonString = JsonConvert.SerializeObject(
                gameMove, 
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            await _socketService.BroadcastMessage(jsonString);

            return CreatedAtAction("GetGameMove", new { id = gameMove.ID }, gameMove);
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