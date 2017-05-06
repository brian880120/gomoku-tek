using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tek.Gomoku.AlphaBeta
{
    public interface IAlphaBetaAlgorithm
    {
        Move MakeDecision(Move[][] board_state);
    }
}
