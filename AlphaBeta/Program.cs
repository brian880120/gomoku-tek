using System;

namespace AlphaBeta
{
    class Move
    {
        public int rowIndex;
        public int colIndex;
        public String color;
        public Boolean directionV = true;
        public Boolean directionH = true;
        public Boolean directionF = true;
        public Boolean directionB = true;

        public Move(int row, int col, String color)
        {
            this.rowIndex = row;
            this.colIndex = col;
            this.color = color;
        }
    }

    class Program
    {
        /*static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }*/


        static void Main(String[] args)
        {
            Move[][] board = new Move[16][];
            for(int i=0; i<16; i++)
            {
                board[i] = new Move[19];
            }
            board[3] [5] = new Move(3, 5, "black");
            Move m = makeDecision(board);
            Console.WriteLine("decision is " + m.colIndex + "    " + m.rowIndex + "     " + m.color);
        }


        public static Move[][] CloneBoard(Move[][] board_state)
        {
            
            Move[][] result = new Move[16][];
            for (int k = 0; k < 16; k++)
            {
                result[k] = new Move[19];
                for (int m = 0; m < 19; m++)
                {
                    result[k][m] = board_state[k][m];
                }
                    
            }
            return result;
        }

        // can be improved if when do a move, compare the move index with min/max Row and min/max Col.
        public static int[] FindBoarder(Move[][] board_state)
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

        public static Move makeDecision(Move[][] board_state)
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
                    Move[][] cloned_board_state = CloneBoard(board_state);
                    

                    if (board_state[i][j] == null)
                    {
                        Move m1 = new Move(i - 1, j - 1, "white");
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
            return new Move(bestRow, bestCol, "white");

        }

        public static double evaluation(Move[][] board_state)
        {

            // debugging ********************
            for(int i=0; i<16; i++)
            {
                for(int j=0; j<19; j++)
                {
                    if (board_state[i][j] != null)
                        Console.WriteLine(board_state[i][j].rowIndex + "   " + board_state[i][j].colIndex + "   " + board_state[i][j].color);
                }
            }

            //*********************

            int whiteScore = 0;
            int blackScore = 0;
            for (int i = 0; i <16; i++)
            {
                for (int j = 0; j <19; j++)
                {
                    if(board_state[i][j] != null)
                    {
                        if (board_state[i][j].color.Equals("white"))
                        {
                            Move mWhite = new Move(i, j, "white");
                            whiteScore += CheckHorizontal(mWhite, board_state);
                            whiteScore += CheckVertical(mWhite, board_state);
                            whiteScore += CheckForwardSlash(mWhite, board_state);
                            whiteScore += CheckBackwardSlash(mWhite, board_state);
                        }
                        else
                        {
                            Move mBlack = new Move(i, j, "black");
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




        public static double alpha_beta(Move[][] board_state, int depth, double a, double b, String color, int[] boarder)
        {
            int minRow = boarder[0]; int maxRow = boarder[1]; int minCol = boarder[2]; int maxCol = boarder[3];
            Boolean moveLeft = false;
            //check number of legal move left
            // can be improved if set a counter and plus one for each time a move added to the boarder
            Boolean outer = true;
            for (int i = minRow; i <= maxRow && outer; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (board_state[i][j] == null)
                    {
                        moveLeft = true;
                        // break out of the outer loop
                        outer = false;
                    }
                }
            }
            // base case
            if (depth == 0 || moveLeft == false)
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
                            Move[][] cloned_board_state2 = CloneBoard(board_state);

                            cloned_board_state2[i][j] = new Move(i - 1, j - 1, "white");

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
                            //Move[][] cloned_board_state2 = CloneBoard(board_state);
                            Move[][] cloned_board_state2 = CloneBoard(board_state);

                            cloned_board_state2[i][j] = new Move(i - 1, j - 1, "black");

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

        public static int CheckHorizontal(Move move, Move[][] occupiedPosition)
        {
            int counter = 1;

            int col = move.colIndex; int row = move.rowIndex; String color = move.color;

            for (int i = col + 1; i <= 18; i++)
            {
                Move temp = occupiedPosition[row][i];
                if (temp!=null && temp.color.CompareTo(color) == 0 && temp.directionH)
                {
                    //if (IsSame(row, i, color, occupiedPosition))
                    counter++;
                    temp.directionH = false;
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
                Move temp = occupiedPosition[row][i];
                if (temp != null && temp.color.CompareTo(color) == 0 && temp.directionH)
                {
                    //if (IsSame(row, i, color, occupiedPosition))
                    counter++;
                    temp.directionH = false;
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


        public static int CheckVertical(Move move, Move[][] occupiedPosition)
        {
            int counter = 1;
            int col = move.colIndex; int row = move.rowIndex; String color = move.color;

            for (int i = row + 1; i <= 15; i++)
            {
                Move temp = occupiedPosition[i][col];
                if (temp!=null && temp.color.CompareTo(color) == 0 && temp.directionV)
                {
                    //if (IsSame(i, col, color, occupiedPosition))
                    counter++;
                    temp.directionV = false;
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
                Move temp = occupiedPosition[i][col];
                if (temp!=null && temp.color.CompareTo(color) == 0 && temp.directionV)
                {
                    //if (IsSame(i, col, color, occupiedPosition))
                    counter++;
                    temp.directionV = false;
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



        public static int CheckForwardSlash(Move move, Move[][] occupiedPosition)
        {
            int counter = 1;
            int col = move.colIndex; int row = move.rowIndex; String color = move.color;
            int j = row + 1; int k = row - 1;

            for (int i = col + 1; i <= 18; i++)
            {
                if (j <= 15)
                {
                    Move temp = occupiedPosition[j][i];
                    if (temp!=null && temp.color.CompareTo(color) == 0 && temp.directionF)
                    {
                        //if (IsSame(j, i, color, occupiedPosition))
                        counter++;
                        temp.directionF = false;
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
                    Move temp = occupiedPosition[k][i];
                    if (temp!=null && temp.color.CompareTo(color) == 0 && temp.directionF)
                    {
                        //if (IsSame(k, i, color, occupiedPosition))
                        counter++;
                        temp.directionF = false;
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


        public static int CheckBackwardSlash(Move move, Move[][] occupiedPosition)
        {
            int counter = 1;
            int col = move.colIndex; int row = move.rowIndex; String color = move.color;
            int j = row - 1; int k = row + 1;

            for (int i = col + 1; i <= 18; i++)
            {
                if (j >= 1)
                {
                    Move temp = occupiedPosition[j][i];
                    if (temp!=null && temp.color.CompareTo(color) == 0 && temp.directionB)
                    {
                        //if (IsSame(j, i, color, occupiedPosition))
                        counter++;
                        temp.directionB = false;
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
                    Move temp = occupiedPosition[k][i];
                    if (temp!=null && temp.color.CompareTo(color) == 0 && temp.directionB)
                    {
                        //if (IsSame(k, i, color, occupiedPosition))
                        counter++;
                        temp.directionB = false;
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