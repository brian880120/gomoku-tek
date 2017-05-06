using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.AlphaBeta;
using Tek.Gomoku.Engine;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public class GameService : IGameService
    {
        private readonly GameContext _context;
        private readonly ISocketService _socket;
        private readonly IJudgementService _judgement;
        private readonly IAlphaBetaAlgorithm _alphaBetaAlgorithm;
        private readonly IConfigurationRoot _config;
        private readonly IEngine _engine;
        private readonly IDataAdapter _adapter;

        public GameService(
            GameContext context,
            ISocketService socket,
            IJudgementService judgement,
            IAlphaBetaAlgorithm alphaBetaAlgorithm,
            IEngine engine,
            IDataAdapter adapter,
            IConfigurationRoot config)
        {
            _context = context;
            _socket = socket;
            _judgement = judgement;
            _alphaBetaAlgorithm = alphaBetaAlgorithm;
            _engine = engine;
            _adapter = adapter;
            _config = config;
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
            var game = await GetGame();
            if (game.Status != GameStatus.Playing) return;

            // Call victoria engine
            var board = _adapter.FromGameMoves(_context.GameMove.ToArray());
            var cordinate = _engine.FindBestMove(board, Color.White, TimeSpan.FromSeconds(10));
            var autoMove = _adapter.ToGameMove(cordinate, Color.White);

            // Call simple alpha-beta algorithm
            //AutoPlayMove[][] board = _adapter.GameMovesToAutoPlayMoves(_context.GameMove.ToArray());
            //AutoPlayMove decision = _autoPlayService.MakeDecision(board);
            //var autoMove = _adapter.AutoPlayMovesToGameMove(decision);

            _context.GameMove.Add(autoMove);

            if (await Judge(autoMove))
            {
                game.Status = GameStatus.WhiteSideWon;
            }
            else
            {
                game.NextPlayer = game.BlackSidePlayer;
            }

            await _context.SaveChangesAsync();

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

        private bool _busy;

        public async Task Move(string userName, GameMove move)
        {
            if (_busy)
            {
                throw new InvalidOperationException("Server is busy processing a move!");
            }

            try
            {
                _busy = true;

                if (_context.GameMove.Any(p => p.RowIndex == move.RowIndex && p.ColumnIndex == move.ColumnIndex))
                {
                    throw new InvalidOperationException("Invalid move!");
                }

                var game = await GetGame();
                var manToMachine = game.ManToMachine;

                await ManualPlay(userName, move);

                if (manToMachine)
                {
                    await AutoPlay();
                }
            }
            finally
            {
                _busy = false;
            }
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

        public async Task SignIn(string userName, bool manToMachine)
        {
            var game = await GetGame();
            if (game.Status != GameStatus.Initial)
            {
                throw new InvalidOperationException("Game has started!");
            }

            game.ManToMachine = game.ManToMachine ? true : manToMachine;

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
    }
}
