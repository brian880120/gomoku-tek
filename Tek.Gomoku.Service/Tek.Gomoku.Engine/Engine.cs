using System;

namespace Tek.Gomoku.Engine
{
    public class Engine : IEngine
    {

        class First
        {
            public int pv, ps0, ps1, ps2, ps3; //Evaluations in 4 directions and their sum
            public int i;       //In which there is a list of good moves
            public int nxt, pre; //The next and previous elements of the good moves list
        };

        struct Tsquare
        {
            public int z;       //0 = blank, 1 = white, 2 = black, 3 = window boundaries
            public First[] h;  //Rating for both players
            public int x, y;        //Coordinate
        };

        int
            player, //0 = white, 1 = black
            moves,  //Number of strokes
            width,  //The width of the playing field
            height, //Height of the playing field
            height2;//height+2
        int[] diroff = new int[9]; //Distance to the adjacent field in the playing field

        //---------------------------------------------------------------------------
        const int  //Constants for the evaluation function
            H10 = 2, H11 = 6, H12 = 10,
            H20 = 23, H21 = 158, H22 = 175,
            H30 = 256, H31 = 511, H4 = 2047;
        int[,] priority = { { 0, 1, 1 }, { H10, H11, H12 }, { H20, H21, H22 }, { H30, H31, 0 }, { H4, 0, 0 } };

        int[] sum = { 0, 0 };   //Total sums of priority of all fields for both players
        int dpth = 0, depth; //Depth of recursion
        int[] D = { 7, 5, 4 };

        const int McurMoves = 384, MwinMoves = 400;
        int[]
            curMoves = new int[McurMoves],  //Buffer for processed strokes
            winMoves1 = new int[MwinMoves], //Buffer on the winning combination
            winEval = new int[MwinMoves];

        int[,]
            goodMoves = new int[4, 2],  //Lists of highly rated fields; For both players
            winMove = new int[2, 2];        //A place where one can win; For both players

        int
            UwinMoves,
            lastMove,
            bestMove;  //The resulting stroke of the computer

        Tsquare[] board;  //playing area
        int boardk; //End of the board
        Random generator;
        //---------------------------------------------------------------------------
        int max(int a, int b)
        {
            return (a > b) ? a : b;
        }
        int abs(int x)
        {
            return x >= 0 ? x : -x;
        }
        int distance(int p1, int p2)
        {
            return max(abs(board[p1].x - board[p2].x),
                abs(board[p1].y - board[p2].y));
        }
        //---------------------------------------------------------------------------
        public Cordinate FindBestMove(Color[,] playingBoard, Color color, TimeSpan remainingTime)
        {
            int p, x, y;

            //Set the depth of thought according to the remaining time
            depth = 4 + (int)remainingTime.TotalSeconds / 60;
            //On first move, see if I'm white or black
            if (moves == 0)
            {
                player = 0;
                if (color == Color.White) player = 1;
            }
            //Find the last opponent's move in the field
            p = 6 * height2 + 1;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    switch (playingBoard[y, x])
                    {
                        case Color.White:
                            if (board[p].z != 1)
                            {
                                doMove(p);
                                goto mujTah;
                            }
                            break;
                        case Color.Black:
                            if (board[p].z != 2)
                            {
                                doMove(p);
                                goto mujTah;
                            }
                            break;
                    }
                    p++;
                }
                p += 2;
            }
            //The opponent's turn is not found => I start	
            player = 0;
            if (color == Color.Black) player = 1;
            //Make your move
            mujTah:
            computer1();
            return new Cordinate(board[lastMove].y, board[lastMove].x);
        }
        //---------------------------------------------------------------------------

        public Engine()
        {
            int x, y, k;
            int p;
            First pr;

            generator = new Random();
            //Allocate the playing area
            width = height = Cordinate.VelikostPlochy;
            height2 = height + 2;
            board = new Tsquare[(width + 12) * (height2)]; //One - dimensional field!
            boardk = (width + 6) * height2;
            //Offsets to move in all eight directions
            diroff[0] = 1;
            diroff[4] = -diroff[0];
            diroff[1] = (1 + height2);
            diroff[5] = -diroff[1];
            diroff[2] = height2;
            diroff[6] = -diroff[2];
            diroff[3] = (-1 + height2);
            diroff[7] = -diroff[3];
            diroff[8] = 0;

            //Clear field
            p = 0;
            for (x = -5; x <= width + 6; x++)
            {
                for (y = 0; y <= height + 1; y++)
                {
                    board[p].z = (x < 1 || y < 1 || x > width || y > height) ? 3 : 0;
                    board[p].x = x - 1;
                    board[p].y = y - 1;
                    board[p].h = new First[2];
                    for (k = 0; k < 2; k++)
                    {
                        board[p].h[k] = pr = new First();
                        pr.i = 0;
                        pr.pv = 4;
                        pr.ps0 = pr.ps1 = pr.ps2 = pr.ps3 = 1;
                    }
                    p++;
                }
            }
            moves = 0;
            //Create an auxiliary table to accelerate the evaluation function
            gen();

        }

        //---------------------------------------------------------------------------
        //Pulls the field p
        bool doMove(int p)
        {
            if (board[p].z != 0) return false;
            board[p].z = player + 1;
            player = 1 - player;
            //Increase stroke counter
            moves++;
            //Override the rating
            evaluate(p);
            lastMove = p;
            return true;
        }
        //---------------------------------------------------------------------------
        short[,] K = new short[2, 262144]; //Rating for all combinations of 9 fields
        static int[] comb = new int[10];
        static int ind;
        static int[] n = new int[4];
        //---------------------------------------------------------------------------
        void gen2(int pos)
        {
            int pb, pe, a1, a2;
            int n1, n2, n3;
            int s;

            if (pos == 9)
            {
                a1 = a2 = 0;
                if (comb[4] == 0)
                {
                    n1 = n[1]; n2 = n[2]; n3 = n[3];
                    pb = 0;
                    pe = 4;
                    while (pe != 9)
                    {
                        if (n3 == 0)
                        {
                            if (n2 == 0)
                            {
                                s = 0;
                                if (comb[pb] == 0 && comb[pe + 1] < 2 && pb != 4)
                                {
                                    s++;
                                    if (comb[pe] == 0 && pe != 4) s++;
                                }
                                int pri = priority[n1, s];
                                if (a1 < pri) a1 = pri;
                            }
                            if (n1 == 0)
                            {
                                s = 0;
                                if (comb[pb] == 0 && (comb[pe + 1] & 1) == 0 && pb != 4)
                                {
                                    s++;
                                    if (comb[pe] == 0 && pe != 4) s++;
                                }
                                int pri = priority[n2, s];
                                if (a2 < pri) a2 = pri;
                            }
                        }
                        switch (comb[++pe])
                        {
                            case 1: n1++; break;
                            case 2: n2++; break;
                            case 3: n3++; break;
                        }
                        switch (comb[pb++])
                        {
                            case 1: n1--; break;
                            case 2: n2--; break;
                            case 3: n3--; break;
                        }
                    }
                }
                K[0, ind] = (short)a1;
                K[1, ind] = (short)a2;
                ind++;
            }
            else
            {
                //Generate all combinations in sequence
                for (int z = 0; z < 4; z++)
                {
                    comb[pos] = z;
                    gen2(pos + 1);
                }
            }
        }

        void gen1(int pos)
        {
            if (pos == 5) gen2(pos);
            else
            {
                for (int z = 0; z < 4; z++)
                {
                    comb[pos] = z;
                    n[z]++;
                    gen1(pos + 1);
                    n[z]--;
                }
            }
        }

        void gen()
        {
            ind = 0;
            gen1(0);
        }
        //---------------------------------------------------------------------------
        //Counts the fields at a distance of 4 from the p0 field
        void evaluate(int p0)
        {
            int i, k, m, s, h;
            First pr;
            int p, q, qk, pe, pk1;
            int ind;
            int pattern;

            //Remove the filled field from the list and give it a zero priority
            if (board[p0].z != 0)
            {
                for (k = 0; k < 2; k++)
                {
                    pr = board[p0].h[k];
                    if (pr.pv != 0)
                    {
                        if (pr.i != 0)
                        {
                            board[pr.nxt].h[k].pre = pr.pre;
                            if (pr.pre != 0) board[pr.pre].h[k].nxt = pr.nxt;
                            else goodMoves[pr.i, k] = pr.nxt;
                            pr.i = 0;
                        }
                        sum[k] -= pr.pv;
                        pr.pv = pr.ps0 = pr.ps1 = pr.ps2 = pr.ps3 = 0;
                    }
                }
            }
            // Process all 4 directions
            for (i = 0; i < 4; i++)
            {
                s = diroff[i];
                pk1 = p0;
                pk1 += s * 5;
                pe = p0;
                p = p0;
                for (m = 4; m > 0; m--)
                {
                    p -= s;
                    if (board[p].z == 3)
                    {
                        pe += s * m;
                        p += s;
                        break;
                    }
                }
                pattern = 0;
                qk = pe;
                qk -= s * 9;
                for (q = pe; q != qk; q -= s)
                {
                    pattern *= 4;
                    pattern += board[q].z;
                }
                while (board[p].z != 3)
                {
                    if (board[p].z == 0)
                    {
                        for (k = 0; k < 2; k++)
                        { //For both players
                            pr = board[p].h[k];
                            //Repairs priority in one direction
                            h = K[k, pattern];
                            switch (i)
                            {
                                case 0:
                                    m = pr.ps0; pr.ps0 = h;
                                    break;
                                case 1:
                                    m = pr.ps1; pr.ps1 = h;
                                    break;
                                case 2:
                                    m = pr.ps2; pr.ps2 = h;
                                    break;
                                case 3:
                                    m = pr.ps3; pr.ps3 = h;
                                    break;
                            }
                            m = h - m;
                            if (m != 0)
                            {
                                sum[k] += m;
                                pr.pv += m;
                                //oprav prioritu v jednom smìru
                                ind = 0;
                                if (pr.pv >= H21)
                                {
                                    ind++;
                                    if (pr.pv >= 2 * H21)
                                    {
                                        ind++;
                                        if (pr.pv >= H4) ind++;
                                    }
                                }
                                //Move the check box to another list
                                if (ind != pr.i)
                                {
                                    //Disconnect
                                    if (pr.i != 0)
                                    {
                                        board[pr.nxt].h[k].pre = pr.pre;
                                        if (pr.pre != 0) board[pr.pre].h[k].nxt = pr.nxt;
                                        else goodMoves[pr.i, k] = pr.nxt;
                                    }
                                    //Connect
                                    if ((pr.i = ind) != 0)
                                    {
                                        q = pr.nxt = goodMoves[ind, k];
                                        goodMoves[ind, k] = board[q].h[k].pre = p;
                                        pr.pre = 0;
                                    }
                                }
                            }
                        }
                    }
                    p += s;
                    if (p == pk1) break;
                    //Rotate pattern to the right; From left to next field
                    pe += s;
                    pattern >>= 2;
                    pattern += board[pe].z << 16;
                }
            }
        }
        //---------------------------------------------------------------------------
        //Main recursive functions
        //Find out if I lose or win
        //At dpth == 0 sets the variable stroke
        int alfabeta(int player1, int UcurMoves, int logWin, int last, int strike)
        {
            int p, q, t, defendMoves1, defendMoves2, UwinMoves0;
            int y, m;
            int i, j, s;
            int pr, hr;
            int mustDefend, mustAttack;

            //When there are four in the line, so stretch without thinking
            p = goodMoves[3, player1];
            if (p != 0)
            {
                if (logWin != 0 && (strike & 1) != 0) winMoves1[UwinMoves++] = p;
                return 1000 - dpth; //I won
            }
            int player2 = 1 - player1;
            p = goodMoves[3, player2];
            if (p != 0)
            {
                board[p].z = player1 + 1;
                evaluate(p);
                if ((strike & 1) != 0)
                    y = -alfabeta(player2, UcurMoves, logWin, last, 2);
                else
                    y = -alfabeta(player2, UcurMoves, logWin, last, 1);
                board[p].z = 0;
                evaluate(p);
                if (logWin != 0 && y != 0 && ((y > 0) == ((strike & 1) != 0))) winMoves1[UwinMoves++] = p;
                return y;
            }

            //First find all good moves and overlay them into a static field
            int Utahy0 = UcurMoves;
            if ((strike & 1) == 0) hr = player2; else hr = player1;
            mustDefend = mustAttack = 0;
            p = goodMoves[2, player1];
            if (p != 0)
            {
                mustAttack++;
                do
                {
                    //I already have three in the row => I should win
                    if (logWin == 0 && board[p].h[player1].pv >= H31)
                    {
                        if (dpth == 0) bestMove = p;
                        return 999 - dpth;
                    }
                    if (UcurMoves == McurMoves) break;
                    pr = board[p].h[hr].pv;
                    for (q = UcurMoves++; q > Utahy0 &&
                        board[curMoves[q - 1]].h[hr].pv < pr; q--)
                    {
                        curMoves[q] = curMoves[q - 1];
                    }
                    curMoves[q] = p;
                    p = board[p].h[player1].nxt;
                } while (p != 0);
            }
            defendMoves1 = UcurMoves;
            for (p = goodMoves[2, player2]; p != 0; p = board[p].h[player2].nxt)
            {
                //The opponent has three in a row => I have to defend myself
                if (board[p].h[player2].pv >= H30 + H21)
                {
                    if (mustDefend == 0) mustDefend = 1;
                    if (board[p].h[player2].pv >= H31) mustDefend = 2;
                }
                else
                {
                    if (mustAttack != 0) continue;
                }
                if (UcurMoves == McurMoves) break;
                pr = board[p].h[hr].pv;
                for (q = UcurMoves++; q > defendMoves1 &&
                    board[curMoves[q - 1]].h[hr].pv < pr; q--)
                {
                    curMoves[q] = curMoves[q - 1];
                }
                curMoves[q] = p;
            }
            defendMoves2 = UcurMoves;

            if (dpth < depth)
            {
                //Just look around the last turn
                if (strike < 2 && last != 0)
                {
                    for (i = 0; i < 8; i++)
                    {
                        s = diroff[i];
                        p = last;
                        p += s;
                        for (j = 0; j < 4 && (board[p].z != 3); j++, p += s)
                        {
                            if ((strike & 1) == 0 && board[p].h[player2].i == 1
                                && (mustAttack == 0 || board[p].h[player2].pv >= H30)
                                || board[p].h[player1].i == 1 &&
                                (mustDefend == 0 || board[p].h[player1].pv >= H30))
                            {
                                if (UcurMoves < McurMoves)
                                {
                                    pr = board[p].h[hr].pv;
                                    for (q = UcurMoves++; q > defendMoves2 &&
                                        board[curMoves[q - 1]].h[hr].pv < pr; q--)
                                    {
                                        curMoves[q] = curMoves[q - 1];
                                    }
                                    curMoves[q] = p;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //defense
                    if (strike == 2 && mustDefend < 2)
                    {
                        for (p = goodMoves[1, player2]; p != 0; p = board[p].h[player2].nxt)
                        {
                            if (UcurMoves == McurMoves) break;
                            if ((last == 0 || distance(p, last) < D[mustDefend])
                                && (mustAttack == 0 || board[p].h[player2].pv >= H30)
                                )
                            {
                                if (UcurMoves == McurMoves) break;
                                pr = board[p].h[hr].pv;
                                for (q = UcurMoves++; q > defendMoves2 &&
                                    board[curMoves[q - 1]].h[hr].pv < pr; q--)
                                {
                                    curMoves[q] = curMoves[q - 1];
                                }
                                curMoves[q] = p;
                            }
                        }
                        defendMoves2 = UcurMoves;
                    }
                    //attack
                    for (p = goodMoves[1, player1]; p != 0; p = board[p].h[player1].nxt)
                    {
                        if (UcurMoves == McurMoves) break;
                        if ((last == 0 || distance(p, last) < 7)
                            && (mustDefend == 0 || board[p].h[player1].pv >= H30)
                            )
                        {
                            if (UcurMoves == McurMoves) break;
                            pr = board[p].h[hr].pv;
                            for (q = UcurMoves++; q > defendMoves2 &&
                                board[curMoves[q - 1]].h[hr].pv < pr; q--)
                            {
                                curMoves[q] = curMoves[q - 1];
                            }
                            curMoves[q] = p;
                        }
                    }
                }
            }

            if (Utahy0 == UcurMoves)
                return 0; //I can not attack anywhere or I'm too deep

            //Good moves are in the curMoves => field and select the best
            UwinMoves0 = UwinMoves;
            m = -0x7ffe;
            for (t = Utahy0; t < UcurMoves; t++)
            {
                dpth++;
                p = curMoves[t];
                //Make a move
                board[p].z = player1 + 1;
                evaluate(p);
                //Recursion 
                if ((strike & 1) != 0)
                {
                    if (t >= defendMoves2 || t < defendMoves1)
                        //Assault move, update the site of the last attack
                        y = -alfabeta(player2, UcurMoves, logWin, p, 0);
                    else
                        //Defensive move, do not change the last attack field 
                        //The opponent gains extra play and can defend where he wants to 
                        y = -alfabeta(player2, UcurMoves, logWin, last, 2);
                }
                else
                {
                    y = -alfabeta(player2, UcurMoves, logWin, last, 1);
                }
                //Delete what you've added 
                board[p].z = 0;
                evaluate(p);
                dpth--;
                if (y > 0)
                {
                    // I win
                    if (dpth == 0) bestMove = p;
                    if (logWin != 0 && (strike & 1) != 0) winMoves1[UwinMoves++] = p;
                    return y;
                }
                if (y == 0)
                {
                    if ((strike & 1) == 0)
                    {
                        //I'm defending myself 
                        UwinMoves = UwinMoves0;
                        if (dpth == 0) bestMove = p;
                        return y;
                    }
                    m = y;
                }
                else if (y >= m)
                {
                    //I have to play, I have to try another possible move
                    if (logWin != 0 && (strike & 1) == 0) { winMoves1[UwinMoves++] = p; logWin = 0; }
                    if (dpth == 0)
                        //Choose a move to lose as late as possible
                        if (y > m || board[p].h[player2].pv > board[bestMove].h[player2].pv)
                            bestMove = p;
                    m = y;
                }
            }
            return m;
        }
        //---------------------------------------------------------------------------
        int try4(int player1, int last)
        {
            int i, j, s;
            int p, p2 = 0, y = 0;

            p = goodMoves[3, player1];
            if (p != 0)
            {
                winMoves1[UwinMoves++] = p;
                return p; //I won
            }
            int player2 = 1 - player1;

            for (i = 0; i < 8; i++)
            {
                s = diroff[i];
                p = last;
                p += s;
                for (j = 0; j < 4 && (board[p].z != 3); j++, p += s)
                {
                    if (board[p].h[player1].pv >= H30)
                    {
                        //attack
                        board[p].z = player1 + 1;
                        evaluate(p);
                        if (goodMoves[3, player2] == 0)
                        {
                            p2 = goodMoves[3, player1];
                            if (p2 != 0)
                            {
                                //Defense - only one option
                                board[p2].z = 2 - player1;
                                evaluate(p2);
                                //Recursion
                                y = try4(player1, p);
                                board[p2].z = 0;
                                evaluate(p2);
                            }
                        }
                        board[p].z = 0;
                        evaluate(p);
                        if (y != 0)
                        {
                            winMoves1[UwinMoves++] = p2;
                            winMoves1[UwinMoves++] = p;
                            return p;
                        }
                    }
                }
            }
            return 0;
        }
        //---------------------------------------------------------------------------
        //Try only forced moves when the attacker does only a quarter
        //The depth of recursion is not limited
        int try4(int player1)
        {
            int p, p2, y = 0, t;
            int j;

            UwinMoves = 0;
            t = 0;
            for (j = 1; j <= 2; j++)
            {
                for (p = goodMoves[j, player1]; p != 0; p = board[p].h[player1].nxt)
                {
                    if (board[p].h[player1].pv >= H30)
                    {
                        if (t == McurMoves) break;
                        curMoves[t++] = p;
                    }
                }
            }
            for (t--; t >= 0; t--)
            {
                p = curMoves[t];
                board[p].z = player1 + 1;
                evaluate(p);
                if (goodMoves[3, 1 - player1] == 0)
                {
                    p2 = goodMoves[3, player1];
                    if (p2 != 0)
                    {
                        board[p2].z = 2 - player1;
                        evaluate(p2);
                        y = try4(player1, p);
                        board[p2].z = 0;
                        evaluate(p2);
                    }
                    board[p].z = 0;
                    evaluate(p);
                    if (y != 0)
                    {
                        winMoves1[UwinMoves++] = p2;
                        winMoves1[UwinMoves++] = p;
                        return p;
                    }
                }
            }
            return 0;
        }
        //---------------------------------------------------------------------------
        int alfabeta(int strike, int player1, int logWin, int last)
        {
            return alfabeta(player1, 0, logWin, last, strike);
        }
        //---------------------------------------------------------------------------
        //Check out the p0 checkbox for player1
        int getEval(int player1, int p0)
        {
            int i, s, y, c1, c2, n;
            int p;

            y = 0;
            //Look at the surrounding field
            c1 = c2 = 0;
            for (i = 0; i < 8; i++)
            {
                s = diroff[i];
                p = p0;
                p += s;
                if (board[p].z == player1 + 1) c1++;
                if (board[p].z == 2 - player1) c2++;
            }
            n = 0;
            if (board[p0].h[player1].ps0 < 2) n++;
            if (board[p0].h[player1].ps1 < 2) n++;
            if (board[p0].h[player1].ps2 < 2) n++;
            if (board[p0].h[player1].ps3 < 2) n++;
            if (n > 2) y -= 8;
            if (c1 + c2 == 0) y -= 20;
            if (c2 == 0 && c1 > 0 && board[p0].h[player1].pv > 9)
            {
                y += (c1 + 1) * 5;
            }
            if (board[p0].h[1 - player1].pv < 5)
            {
                n = 0;
                if (board[p0].h[player1].ps0 >= H12) n++;
                if (board[p0].h[player1].ps1 >= H12) n++;
                if (board[p0].h[player1].ps2 >= H12) n++;
                if (board[p0].h[player1].ps3 >= H12) n++;
                y += 15;
                if (n > 1) y += n * 64;
            }
            return y + board[p0].h[player1].pv;
        }
        //---------------------------------------------------------------------------
        int getEval(int p)
        {
            int a, b;
            a = getEval(0, p);
            b = getEval(1, p);
            //Combine the rating of both players
            return a > b ? a + b / 2 : a / 2 + b;
        }
        //---------------------------------------------------------------------------
        //Defense, try to drag the fields from the winMoves1 field
        int defend(int player1)
        {
            int p, t;
            int m, mv, mh, y, yh, Nwins, i, j;
            int player2 = 1 - player1;
            int th, thm = 0;

            dpth++;
            Nwins = UwinMoves;
            //Count all check boxes in the list
            for (t = UwinMoves - 1, th = Nwins - 1; t != -1; t--, th--)
            {
                winEval[th] = getEval(player2, winMoves1[t]);
            }

            mh = m = -0x7ffe;
            for (i = 0; Nwins > 0 && i < 20; i++)
            {
                //Select the checkbox with the highest rating
                mv = -0x7ffe;
                for (th = Nwins - 1; th != -1; th--)
                {
                    if (winEval[th] > mv) { thm = th; mv = winEval[th]; }
                }
                if (mv < 25) break;
                //Remove him from the list
                j = thm;
                p = winMoves1[j];
                Nwins--;
                winMoves1[j] = winMoves1[Nwins];

                board[p].z = player1 + 1;
                evaluate(p);
                y = -alfabeta(3, player2, 0, 0);
                board[p].z = 0;
                evaluate(p);
                yh = winEval[thm] + y * 20;
                if (yh > mh)
                {
                    m = y;
                    mh = yh;
                    bestMove = p;
                    if (y > 0 || y == 0 && winMove[player, player1] == 0) break;
                }
                winEval[thm] = winEval[Nwins];
            }
            if (m < 0)
            {
                //When I lose, I'll try to attack (sometimes it will help)
                t = 0;
                for (p = goodMoves[1, player1]; p != 0; p = board[p].h[player1].nxt)
                {
                    if (board[p].h[player1].pv >= H30)
                    {
                        if (t == MwinMoves) break;
                        winMoves1[t++] = p;
                    }
                }
                for (t--; t >= 0; t--)
                {
                    p = winMoves1[t];
                    board[p].z = player1 + 1;
                    evaluate(p);
                    y = -alfabeta(3, player2, 0, 0);
                    board[p].z = 0;
                    evaluate(p);
                    if (y > m)
                    {
                        m = y;
                        bestMove = p;
                        if (y >= 0) break;
                    }
                }
            }
            dpth--;
            return m;
        }
        //---------------------------------------------------------------------------
        //Finds the box with the highest rating
        //If the rating is too small, it returns 0
        int findMax(int player1)
        {
            int p, t;
            int m, r;
            int i, k;

            m = -1;
            t = 0;
            for (i = 2; i > 0 && t == 0; i--)
                for (k = 0; k < 2; k++)
                    for (p = goodMoves[i, k]; p != 0; p = board[p].h[k].nxt)
                    {
                        r = getEval(p);
                        if (r > m)
                        {
                            m = r;
                            t = p;
                        }
                    }
            return t;
        }
        //---------------------------------------------------------------------------
        // Find out what the overall rating will be in a few turns
        int lookAhead(int player1)
        {
            int p;
            int y;

            if (goodMoves[3, player1] != 0) return 500; //A win was found
            int player2 = 1 - player1;
            p = goodMoves[3, player2];
            if (p == 0 && dpth < 4) p = findMax(player1);
            if (p == 0)
            {
                return (sum[player1] - sum[player2]) / 3;
            }
            dpth++;
            board[p].z = player1 + 1;
            evaluate(p);
            y = -lookAhead(player2);
            board[p].z = 0;
            evaluate(p);
            dpth--;
            return y;
        }
        //---------------------------------------------------------------------------
        void computer1()
        {
            int p;
            int Nresults = 0;
            int m, y = 0, rnd;
            int r;
            int player1 = player, player2 = 1 - player1;

            //The first move will be in the middle of the playing area
            if (moves == 0)
            {
                doMove((width / 2 + 6) * height2 + height / 2 + 1);
                return;
            }
            //Make a second stroke randomly on some adjacent box
            if (moves == 1)
            {
                for (;;) //The first move could be on the edge or in the corner!
                    switch (generator.Next(0, 4))
                    {
                        case 0:
                            if (doMove(lastMove + 1)) return;
                            break;
                        case 1:
                            if (doMove(lastMove - 1)) return;
                            break;
                        case 2:
                            if (doMove(lastMove + height2)) return;
                            break;
                        case 3:
                            if (doMove(lastMove - height2)) return;
                            break;
                    }
            }
            lastMove = -1;

            //When there are four in the line, so stretch without thinking
            if (doMove(goodMoves[3, player1])) return; //I just won
            if (doMove(goodMoves[3, player2])) return; //I have to defend myself

            //Try to do all the possible squares
            bestMove = 0;
            if (doMove(try4(player1))) return; //I will definitely win

            //What if the opponent will do only a quarter
            p = try4(player2);
            if (p != 0)
            {
                // 	The opponent can win => I have to defend myself
                winMove[player1, player2] = p;
                y = 1;
            }
            else
            {
                bestMove = 0;
                if (winMove[player1, player1] != 0)
                {
                    //A win was found in the previous draw
                    y = alfabeta(1, player1, 0, winMove[player1, player1]);
                    if (y <= 0) winMove[player1, player1] = 0; //The opponent managed to defend himself
                }
                //Find out if I can win
                if (bestMove == 0)
                {
                    y = alfabeta(3, player1, 0, 0);
                }
                if (y > 0 && bestMove != 0)
                {
                    //Probably win
                    doMove(bestMove);
                    //Remember this place so that you do not play anywhere else in the next move
                    winMove[player1, player1] = bestMove;
                    return;
                }
                //Find out if your opponent can win
                y = 0;
                if (winMove[player1, player2] != 0)
                {
                    UwinMoves = 0;
                    y = alfabeta(1, player2, 1, winMove[player1, player2]);
                    if (y <= 0)
                    {
                        winMove[player1, player2] = 0; //The opponent could win, but he broke it
                    }
                }
                if (y <= 0)
                {
                    UwinMoves = 0;
                    y = alfabeta(3, player2, 1, 0);
                    if (y > 0) winMove[player1, player2] = bestMove; //I'll probably lose
                }
            }
            bestMove = 0;

            if (y > 0)
            {
                //defense
                if (UwinMoves > 0)
                {
                    //Try to defend yourself only on the boxes where the opponent's win was found
                    defend(player1);
                }
                if (bestMove == 0)
                {
                    //Try to defend yourself anywhere
                    alfabeta(2, player1, 0, 0);
                }
            }

            if (bestMove == 0 && moves > 9)
            {
                //Depth search does not find winning moves
                m = -0x7ffffffe;
                for (p = 0; p < boardk; p++)
                {
                    if (board[p].z == 0 && (board[p].h[0].pv > 10 || board[p].h[1].pv > 10))
                    {
                        r = getEval(p);
                        board[p].z = player1 + 1;
                        evaluate(p);
                        r -= lookAhead(player2);
                        board[p].z = 0;
                        evaluate(p);
                        if (r > m)
                        {
                            m = r;
                            bestMove = p;
                            Nresults = 1;
                        }
                        else if (r > m - 20)
                        {
                            Nresults++;
                            if (generator.Next(0, Nresults) == 0) bestMove = p;
                        }
                    }
                }
            }
            if (bestMove == 0)
            {
                //Select the checkbox with the highest rating
                m = -1;
                for (p = 0; p < boardk; p++)
                {
                    if (board[p].z == 0)
                    {
                        r = getEval(p);
                        if (r > m) m = r;
                    }
                }
                //Randomly select a check box that is rated slightly less than the best
                rnd = m / 12;
                if (rnd > 30) rnd = 30;
                Nresults = 0;
                for (p = 0; p < boardk; p++)
                {
                    if (board[p].z == 0)
                    {
                        if (getEval(p) >= m - rnd)
                        {
                            Nresults++;
                            if (generator.Next(0, Nresults) == 0) bestMove = p;
                        }
                    }
                }

            }
            //Finally make your move
            doMove(bestMove);
        }
        //---------------------------------------------------------------------------

        public void Reset()
        {
        }
    }
}
