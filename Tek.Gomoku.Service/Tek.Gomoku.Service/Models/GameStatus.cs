using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Models
{
    public static class GameStatus
    {
        public static string Initial = "Initial";
        public static string Playing = "Playing";
        public static string BlackSideWon = "BlackSideWon";
        public static string WhiteSideWon = "WhiteSideWon";
    }
}
