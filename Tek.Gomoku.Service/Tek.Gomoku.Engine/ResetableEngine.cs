using System;
using System.Collections.Generic;
using System.Text;

namespace Tek.Gomoku.Engine
{
    public class ResetableEngine : IEngine
    {
        private IEngine _innerEngine = new Engine();

        public Cordinate FindBestMove(Color[,] hraciPole, Color barvaNaTahu, TimeSpan zbyvajiciCasNaPartii)
        {
            return _innerEngine.FindBestMove(hraciPole, barvaNaTahu, zbyvajiciCasNaPartii);
        }

        public void Reset()
        {
            _innerEngine = new Engine();
        }
    }
}
