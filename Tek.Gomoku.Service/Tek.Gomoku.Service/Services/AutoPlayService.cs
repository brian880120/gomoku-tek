﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public class AutoPlayService : IAutoPlayService
    {
        public GameMove MakeDecision(GameMove[] gameMoves)
        {
            // 1. 1d => 2d
            AutoPlayMove[][] board = Adapter.GameMovesToAutoPlayMoves(gameMoves);

            // 2. call your alpha-beta algorithm
            AutoPlayMove decision = makeDecision(board);

            // 3. AutoPlayMove => GameMove
            GameMove output = Adapter.AutoPlayMovesToGameMove(decision);

            // 4. return
            return output; // todo
        }


        public static AutoPlayMove[][] CloneBoard(AutoPlayMove[][] board_state)
        {

            AutoPlayMove[][] result = new AutoPlayMove[16][];
            for (int k = 0; k < 16; k++)
            {
                result[k] = new AutoPlayMove[19];
                for (int m = 0; m < 19; m++)
                {
                    result[k][m] = board_state[k][m];
                }

            }
            return result;
        }

        // can be improved if when do a AutoPlayMove, compare the AutoPlayMove index with min/max Row and min/max Col.
        public static int[] FindBoarder(AutoPlayMove[][] board_state)
        {
            int maxCol = 0; int minCol = 100; int maxRow = 0; int minRow = 100;

            for (int i = 1; i < 16; i++)
            {
                for (int j = 1; j < 19; j++)
                {
                    if (board_state[i][j] != null)
                    {
                        if (j < minCol) minCol = j;
                        if (i < minRow) minRow = i;
                        if (j > maxCol) maxCol = j;
                        if (i > maxRow) maxRow = i;
                    }
                }
            }
            if (minRow > 2) minRow = minRow - 2;
            if (minCol > 2) minCol = minCol - 2;
            if (maxRow < 15) maxRow = maxRow + 2;
            if (maxCol < 17) maxCol = maxCol + 2;

            int[] result = { minRow, maxRow, minCol, maxCol };
            return result;
        }

        public static AutoPlayMove makeDecision(AutoPlayMove[][] board_state)
        {
            double score;
            double bestscore = -1000;
            int depth = 4;

            int bestRow = 0; int bestCol = 0;
            // 
            int[] boarder = FindBoarder(board_state);
            int minRow = boarder[0]; int maxRow = boarder[1]; int minCol = boarder[2]; int maxCol = boarder[3];

            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    // clone a temp board
                    AutoPlayMove[][] cloned_board_state = CloneBoard(board_state);


                    if (board_state[i][j] == null)
                    {
                        AutoPlayMove m1 = new AutoPlayMove(i - 1, j - 1, "white");
                        cloned_board_state[i][j] = m1;

                        score = alpha_beta(cloned_board_state, depth, -1000, 1000, "black", FindBoarder(cloned_board_state));
                        if (score > bestscore)
                        {
                            bestRow = i;
                            bestCol = j;
                            bestscore = score;
                        }
                    }
                }
            }
            return new AutoPlayMove(bestRow, bestCol, "white");

        }

        public static double evaluation(AutoPlayMove[][] board_state)
        {

            // debugging ********************
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (board_state[i][j] != null)
                        Console.WriteLine(board_state[i][j].RowIndex + "   " + board_state[i][j].ColIndex + "   " + board_state[i][j].Color);
                }
            }

            //*********************

            int whiteScore = 0;
            int blackScore = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (board_state[i][j] != null)
                    {
                        if (board_state[i][j].Color.Equals("white"))
                        {
                            AutoPlayMove mWhite = new AutoPlayMove(i, j, "white");
                            whiteScore += CheckHorizontal(mWhite, board_state);
                            whiteScore += CheckVertical(mWhite, board_state);
                            whiteScore += CheckForwardSlash(mWhite, board_state);
                            whiteScore += CheckBackwardSlash(mWhite, board_state);
                        }
                        else
                        {
                            AutoPlayMove mBlack = new AutoPlayMove(i, j, "black");
                            blackScore += CheckHorizontal(mBlack, board_state);
                            blackScore += CheckVertical(mBlack, board_state);
                            blackScore += CheckForwardSlash(mBlack, board_state);
                            blackScore += CheckBackwardSlash(mBlack, board_state);
                        }

                    }


                }
            }
            return whiteScore - blackScore;

        }




        public static double alpha_beta(AutoPlayMove[][] board_state, int depth, double a, double b, String color, int[] boarder)
        {
            int minRow = boarder[0]; int maxRow = boarder[1]; int minCol = boarder[2]; int maxCol = boarder[3];
            Boolean AutoPlayMoveLeft = false;
            //check number of legal AutoPlayMove left
            // can be improved if set a counter and plus one for each time a AutoPlayMove added to the boarder
            Boolean outer = true;
            for (int i = minRow; i <= maxRow && outer; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (board_state[i][j] == null)
                    {
                        AutoPlayMoveLeft = true;
                        // break out of the outer loop
                        outer = false;
                    }
                }
            }
            // base case
            if (depth == 0 || AutoPlayMoveLeft == false)
                return evaluation(board_state);
            else
            {
                if (color.Equals("white"))
                {
                    double max = -1000;
                    double temp_score1;
                    for (int i = minRow; i <= maxRow; i++)
                    {
                        for (int j = minCol; j <= maxCol; j++)
                        {

                            // clone 2D array
                            AutoPlayMove[][] cloned_board_state2 = CloneBoard(board_state);

                            cloned_board_state2[i][j] = new AutoPlayMove(i - 1, j - 1, "white");

                            temp_score1 = alpha_beta(cloned_board_state2, depth - 1, a, b, "black", FindBoarder(cloned_board_state2));
                            if (temp_score1 >= max)
                                max = temp_score1;
                            a = max;
                            if (a >= b)
                                return b;
                        }
                    }
                    return max;
                }

                // min for enemy
                else
                {
                    double min = 1000;
                    double temp_score2;
                    for (int i = minRow; i <= maxRow; i++)
                    {
                        for (int j = minCol; j <= maxCol; j++)
                        {
                            // clone board
                            //AutoPlayMove[][] cloned_board_state2 = CloneBoard(board_state);
                            AutoPlayMove[][] cloned_board_state2 = CloneBoard(board_state);

                            cloned_board_state2[i][j] = new AutoPlayMove(i - 1, j - 1, "black");

                            temp_score2 = alpha_beta(cloned_board_state2, depth - 1, a, b, "white", FindBoarder(cloned_board_state2));
                            if (temp_score2 >= min)
                                min = temp_score2;
                            a = min;
                            if (a >= b)
                                return b;
                        }
                    }
                    return min;
                }
            }
        }




        // helper for evaluation function

        public static int CheckHorizontal(AutoPlayMove AutoPlayMove, AutoPlayMove[][] occupiedPosition)
        {
            int counter = 1;

            int col = AutoPlayMove.ColIndex; int row = AutoPlayMove.RowIndex; String color = AutoPlayMove.Color;

            for (int i = col + 1; i <= 18; i++)
            {
                AutoPlayMove temp = occupiedPosition[row][i];
                if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionH)
                {
                    //if (IsSame(row, i, color, occupiedPosition))
                    counter++;
                    temp.DirectionH = false;
                }
                else
                {
                    if (temp != null)
                        counter -= 2;
                    break;
                }
            }

            for (int i = col - 1; i >= 1; i--)
            {
                AutoPlayMove temp = occupiedPosition[row][i];
                if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionH)
                {
                    //if (IsSame(row, i, color, occupiedPosition))
                    counter++;
                    temp.DirectionH = false;
                }
                else
                {
                    if (temp != null)
                        counter -= 2;
                    break;
                }
            }

            return counter;
        }


        public static int CheckVertical(AutoPlayMove AutoPlayMove, AutoPlayMove[][] occupiedPosition)
        {
            int counter = 1;
            int col = AutoPlayMove.ColIndex; int row = AutoPlayMove.RowIndex; String color = AutoPlayMove.Color;

            for (int i = row + 1; i <= 15; i++)
            {
                AutoPlayMove temp = occupiedPosition[i][col];
                if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionV)
                {
                    //if (IsSame(i, col, color, occupiedPosition))
                    counter++;
                    temp.DirectionV = false;
                }
                else
                {
                    if (temp != null)
                        counter -= 2;
                    break;
                }
            }

            for (int i = row - 1; i >= 1; i--)
            {
                AutoPlayMove temp = occupiedPosition[i][col];
                if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionV)
                {
                    //if (IsSame(i, col, color, occupiedPosition))
                    counter++;
                    temp.DirectionV = false;
                }
                else
                {
                    if (temp != null)
                        counter -= 2;
                    break;
                }
            }

            return counter;
        }



        public static int CheckForwardSlash(AutoPlayMove AutoPlayMove, AutoPlayMove[][] occupiedPosition)
        {
            int counter = 1;
            int col = AutoPlayMove.ColIndex; int row = AutoPlayMove.RowIndex; String color = AutoPlayMove.Color;
            int j = row + 1; int k = row - 1;

            for (int i = col + 1; i <= 18; i++)
            {
                if (j <= 15)
                {
                    AutoPlayMove temp = occupiedPosition[j][i];
                    if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionF)
                    {
                        //if (IsSame(j, i, color, occupiedPosition))
                        counter++;
                        temp.DirectionF = false;
                    }
                    else
                    {
                        if (temp != null)
                            counter -= 2;
                        break;
                    }
                }
                else
                    break;
                j++;
            }

            for (int i = col - 1; i >= 1; i--)
            {
                if (k >= 1)
                {
                    AutoPlayMove temp = occupiedPosition[k][i];
                    if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionF)
                    {
                        //if (IsSame(k, i, color, occupiedPosition))
                        counter++;
                        temp.DirectionF = false;
                    }
                    else
                    {
                        if (temp != null)
                            counter -= 2;
                        break;
                    }
                }
                else
                    break;
                k--;
            }


            return counter;
        }


        public static int CheckBackwardSlash(AutoPlayMove AutoPlayMove, AutoPlayMove[][] occupiedPosition)
        {
            int counter = 1;
            int col = AutoPlayMove.ColIndex; int row = AutoPlayMove.RowIndex; String color = AutoPlayMove.Color;
            int j = row - 1; int k = row + 1;

            for (int i = col + 1; i <= 18; i++)
            {
                if (j >= 1)
                {
                    AutoPlayMove temp = occupiedPosition[j][i];
                    if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionB)
                    {
                        //if (IsSame(j, i, color, occupiedPosition))
                        counter++;
                        temp.DirectionB = false;
                    }
                    else
                    {
                        if (temp != null)
                            counter -= 2;
                        break;
                    }
                }
                else
                    break;
                j--;
            }

            for (int i = col - 1; i >= 1; i--)
            {
                if (k <= 15)
                {
                    AutoPlayMove temp = occupiedPosition[k][i];
                    if (temp != null && temp.Color.CompareTo(color) == 0 && temp.DirectionB)
                    {
                        //if (IsSame(k, i, color, occupiedPosition))
                        counter++;
                        temp.DirectionB = false;
                    }
                    else
                    {
                        if (temp != null)
                            counter -= 2;
                        break;
                    }
                }
                else
                    break;
                k++;
            }

            return counter;
        }

    }
}
