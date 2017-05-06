using System;

namespace Tek.Gomoku.Engine
{
    public interface IEngine
    {
        void Reset();

        Cordinate FindBestMove(Color[,] playingBoard, Color color, TimeSpan remainingTime);
    }
}
