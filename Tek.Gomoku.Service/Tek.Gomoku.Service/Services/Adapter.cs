using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public class Adapter
    {
        public static GameMove AutoPlayMovesToGameMove(AutoPlayMove autoPlayMove)
        {
            var result = new GameMove();
            result.ColumnIndex = autoPlayMove.ColIndex;
            result.RowIndex = autoPlayMove.RowIndex;
            result.ColorInString = autoPlayMove.Color;

            return result;
        }

        public static AutoPlayMove[][] GameMovesToAutoPlayMoves(GameMove[] gameMoves)
        {
            AutoPlayMove[][] board = new AutoPlayMove[16][];
            for (int i = 0; i < 16; i++)
            {
                board[i] = new AutoPlayMove[19];
            }

            for (int i = 0; i < gameMoves.Length; i++)
            {
                GameMove temp = gameMoves[i];
                int row = temp.RowIndex;
                int col = temp.ColumnIndex;
                String color = temp.ColorInString;
                board[row][col] = new AutoPlayMove(row, col, color);
            }
            return board; // todo
        }
    }
}
