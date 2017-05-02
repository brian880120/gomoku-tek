using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public class GameService : IGameService
    {
        private readonly GameContext _context;
        private readonly ISocketService _socket;
        private readonly IGameJudgementService _judgement;
        private readonly IAutoPlayService _autoPlayService;

        public GameService(
            GameContext context,
            ISocketService socket,
            IGameJudgementService judgement,
            IAutoPlayService autoPlayService)
        {
            _context = context;
            _socket = socket;
            _judgement = judgement;
            _autoPlayService = autoPlayService;
        }

        private async Task<Game> GetGame()
        {
            var game = _context.Game.FirstOrDefault();
            if (game == null)
            {
                game = new Game()
                {
                    Status = GameStatus.Initial
                };
                await _context.Game.AddAsync(game);
            }

            return game;
        }

        private string GetNextPlayerName(string userName, Game game)
        {
            var whitePlayer = game.WhiteSidePlayer;
            var blackPlayer = game.BlackSidePlayer;
            var currentPlayer = userName;
            var nextPlayer = currentPlayer == whitePlayer ? blackPlayer : whitePlayer;
            return nextPlayer;
        }

        private string GetColorInstring(string userName, Game game)
        {
            var colorInString = userName == game.BlackSidePlayer ? "black" : "white";
            return colorInString;
        }

        private async Task<bool> Judge(GameMove move)
        {
            var allMoves = await _context.GameMove.ToArrayAsync();
            var result = _judgement.Check(move, allMoves);
            return result;
        }

        private async Task AutoPlay()
        {
            var game = GetGame();

            var autoMove = _autoPlayService.MakeDecision(_context.GameMove.ToArray());
            _context.GameMove.Add(autoMove);
            _context.SaveChanges();

            var message = new WebSocketMessage()
            {
                Type = "GameMove",
                Payload = autoMove
            };
            await _socket.BroadcastMessage(message);

            message = new WebSocketMessage()
            {
                Type = "Game",
                Payload = game
            };
            await _socket.BroadcastMessage(message);
        }

        private async Task ManualPlay(string userName, GameMove move)
        {
            var game = await GetGame();
            if (game.Status != GameStatus.Playing)
            {
                throw new InvalidOperationException("Game not started yet!");
            }

            if (game.NextPlayer != userName)
            {
                throw new InvalidOperationException("Not your turn!");
            }

            move.ColorInString = GetColorInstring(userName, game);
            await _context.AddAsync(move);

            game.NextPlayer = GetNextPlayerName(userName, game);
            if (await Judge(move))
            {
                game.Status = move.ColorInString == "black" ? GameStatus.BlackSideWon : GameStatus.WhiteSideWon;
            }

            await _context.SaveChangesAsync();

            var message = new WebSocketMessage()
            {
                Type = "GameMove",
                Payload = move
            };
            await _socket.BroadcastMessage(message);

            message = new WebSocketMessage()
            {
                Type = "Game",
                Payload = game
            };
            await _socket.BroadcastMessage(message);
        }

        public async Task Move(string userName, GameMove move)
        {
            await ManualPlay(userName, move);

            await AutoPlay();
        }

        public async Task SignIn(string userName)
        {
            var game = await GetGame();
            if (game.Status != GameStatus.Initial)
            {
                throw new InvalidOperationException("Game has started!");
            }

            if (string.IsNullOrWhiteSpace(game.BlackSidePlayer))
            {
                game.BlackSidePlayer = userName;
            }
            else
            {
                game.WhiteSidePlayer = userName;
            }

            if (!string.IsNullOrWhiteSpace(game.BlackSidePlayer) && !string.IsNullOrWhiteSpace(game.WhiteSidePlayer))
            {
                game.Status = GameStatus.Playing;
                game.NextPlayer = game.BlackSidePlayer;
            }

            await _context.SaveChangesAsync();

            var webSocketMessage = new WebSocketMessage()
            {
                Type = "Game",
                Payload = game
            };
            await _socket.BroadcastMessage(webSocketMessage);
        }

        public async Task SignOut(string userName)
        {
            var game = await GetGame();

            if (game.BlackSidePlayer != userName && game.WhiteSidePlayer != userName)
            {
                throw new InvalidOperationException($"{userName} is not part of the game!");
            }

            if (userName == game.BlackSidePlayer)
            {
                game.BlackSidePlayer = null;
            }
            else if (userName == game.WhiteSidePlayer)
            {
                game.WhiteSidePlayer = null;
            }
            game.NextPlayer = null;
            game.Status = GameStatus.Initial;

            await _context.GameMove.ForEachAsync(p => _context.GameMove.Remove(p));

            await _context.SaveChangesAsync();

            var webSocketMessage = new WebSocketMessage()
            {
                Type = "Game",
                Payload = game
            };
            await _socket.BroadcastMessage(webSocketMessage);
        }
    }
}
