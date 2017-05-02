﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Models
{
    public class AutoPlayMove
    {
        public AutoPlayMove(int colIndex, int rowIndex, string color)
        {
            ColIndex = colIndex;
            RowIndex = rowIndex;
            Color = color;
            DirectionB = true;
            DirectionF = true;
            DirectionH = true;
            DirectionV = true;
        }

        public int ColIndex { get; set; }
        public int RowIndex { get; set; }
        public bool DirectionH { get; set; }
        public bool DirectionV { get; set; }
        public bool DirectionF { get; set; }
        public bool DirectionB { get; set; }
        public string Color { get; set; }
    }
}
