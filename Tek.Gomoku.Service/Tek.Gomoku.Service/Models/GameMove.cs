using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Models
{
    public class GameMove
    {
        public int ID { get; set; }

        public int ColumnIndex { get; set; }

        public int RowIndex { get; set; }

        public string PlayerName { get; set; }

        public string ColorInString { get; set; }
    }
}
