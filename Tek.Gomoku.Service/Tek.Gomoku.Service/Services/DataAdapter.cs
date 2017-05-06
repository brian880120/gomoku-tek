using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Engine;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public class DataAdapter : IDataAdapter
    {
        public GameMove AutoPlayMovesToGameMove(AutoPlayMove autoPlayMove)
        {
            var result = new GameMove();
            result.ColumnIndex = autoPlayMove.ColIndex;
            result.RowIndex = autoPlayMove.RowIndex;
            result.ColorInString = autoPlayMove.Color;

            return result;
        }

        public AutoPlayMove[][] GameMovesToAutoPlayMoves(GameMove[] gameMoves)
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

        public GameMove ToGameMove(Cordinate cordinate, Color color)
        {
            var result = new GameMove()
            {
                RowIndex = cordinate.Row,
                ColumnIndex = cordinate.Column,
                ColorInString = color == Color.White ? "white" : "black"
            };

            return result;
        }

        public Color[,] FromGameMoves(GameMove[] gameMoves)
        {
            var result = new Color[19, 19];

            for (var i = 0; i < 19; i++)
            {
                for (var j = 0; j < 19; j++)
                {
                    result[i, j] = Color.None;
                }
            }

            foreach (var gameMove in gameMoves)
            {
                result[gameMove.RowIndex, gameMove.ColumnIndex] = gameMove.ColorInString == "white" ? Color.White : Color.Black;
            }

            return result;
        }
    }
}
