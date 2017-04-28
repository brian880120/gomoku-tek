using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Models
{
    public class Game
    {
        public int ID { get; set; }

        public string BlackSidePlayer { get; set; }

        public string WhiteSidePlayer { get; set; }

        public string NextPlayer { get; set; }
    }
}

