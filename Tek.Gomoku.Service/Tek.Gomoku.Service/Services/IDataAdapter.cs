using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Engine;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public interface IDataAdapter
    {
        GameMove AutoPlayMovesToGameMove(AutoPlayMove autoPlayMove);

        AutoPlayMove[][] GameMovesToAutoPlayMoves(GameMove[] gameMoves);

        GameMove ToGameMove(Cordinate cordinate, Color color);

        Color[,] FromGameMoves(GameMove[] gameMoves);
    }
}
