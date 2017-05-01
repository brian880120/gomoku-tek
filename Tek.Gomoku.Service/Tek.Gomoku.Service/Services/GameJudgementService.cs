using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public class GameJudgementService : IGameJudgementService
    {
        public bool Check(GameMove gameMove, GameMove[] occupiedPisition)
        {
            return CheckHorizontal(gameMove, occupiedPisition) ||
                CheckVertical(gameMove, occupiedPisition) ||
                CheckBackwardSlash(gameMove, occupiedPisition) ||
                CheckForwardSlash(gameMove, occupiedPisition);
        }

        private Boolean IsSame(int row, int col, String color, GameMove[] occupiedPosition)
        {
            for (int i = 0; i < occupiedPosition.Length; i++)
            {
                if (row == occupiedPosition[i].RowIndex && col == occupiedPosition[i].ColumnIndex && String.Equals(color, occupiedPosition[i].ColorInString))
                    //Console.WriteLine("array length  "+ occupiedPosition.Length + "  " + row + "  " + col + "  " + color);
                    return true;
            }
            return false;
        }

        private Boolean CheckHorizontal(GameMove move, GameMove[] occupiedPosition)
        {
            int counter = 1;
            int col = move.ColumnIndex; int row = move.RowIndex; String color = move.ColorInString;

            for (int i = col + 1; i <= 18; i++)
            {
                if (IsSame(row, i, color, occupiedPosition))
                    counter++;
                else
                    break;
            }

            for (int i = col - 1; i >= 1; i--)
            {
                if (IsSame(row, i, color, occupiedPosition))
                    counter++;
                else
                    break;
            }

            if (counter == 5)
                return true;
            return false;
        }


        private Boolean CheckVertical(GameMove move, GameMove[] occupiedPosition)
        {
            int counter = 1;
            int col = move.ColumnIndex; int row = move.RowIndex; String color = move.ColorInString;

            for (int i = row + 1; i <= 15; i++)
            {
                if (IsSame(i, col, color, occupiedPosition))
                    counter++;
                else
                    break;
            }

            for (int i = row - 1; i >= 1; i--)
            {
                if (IsSame(i, col, color, occupiedPosition))
                    counter++;
                else
                    break;
            }

            if (counter == 5)
                return true;
            return false;
        }



        private Boolean CheckForwardSlash(GameMove move, GameMove[] occupiedPosition)
        {
            int counter = 1;
            int col = move.ColumnIndex; int row = move.RowIndex; String color = move.ColorInString;
            int j = row + 1; int k = row - 1;

            for (int i = col + 1; i <= 18; i++)
            {
                if (j <= 15)
                {
                    if (IsSame(j, i, color, occupiedPosition))
                        counter++;
                    else
                        break;
                }
                else
                    break;
                j++;
            }

            for (int i = col - 1; i >= 1; i--)
            {
                if (k >= 1)
                {
                    if (IsSame(k, i, color, occupiedPosition))
                        counter++;
                    else
                        break;
                }
                else
                    break;
                k--;
            }

            if (counter == 5)
                return true;

            return false;
        }


        private Boolean CheckBackwardSlash(GameMove move, GameMove[] occupiedPosition)
        {
            int counter = 1;
            int col = move.ColumnIndex; int row = move.RowIndex; String color = move.ColorInString;
            int j = row - 1; int k = row + 1;

            for (int i = col + 1; i <= 18; i++)
            {
                if (j >= 1)
                {
                    if (IsSame(j, i, color, occupiedPosition))
                        counter++;
                    else
                        break;
                }
                else
                    break;
                j--;
            }

            for (int i = col - 1; i >= 1; i--)
            {
                if (k <= 15)
                {
                    if (IsSame(k, i, color, occupiedPosition))
                        counter++;
                    else
                        break;
                }
                else
                    break;
                k++;
            }

            if (counter == 5)
                return true;

            return false;
        }
    }
}
