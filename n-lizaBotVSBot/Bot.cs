using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_lizaBotVSBot
{
    class Point{
        public int X;
        public int Y;
        public Point(int X,int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
    abstract class Bot
    {
        abstract public Point CalculateMove(int[,] Board, int Player);
    }
}
