using System;
using System.Collections.Generic;
using System.Text;

namespace Tek.Gomoku.Engine
{
    public class Cordinate
    {
        public const int VelikostPlochy = 19;

        public Cordinate(int row, int column)
        {
            if ((row >= 0) && (row < VelikostPlochy))
                Row = row;
            else
                throw (new Exception("The value of 'row' is out of the limits"));
            if ((column >= 0) && (column < VelikostPlochy))
                Column = column;
            else
                throw (new Exception("The value of 'column' is out of the limits"));
        }

        public readonly int Row;

        public readonly int Column;
    }
}
