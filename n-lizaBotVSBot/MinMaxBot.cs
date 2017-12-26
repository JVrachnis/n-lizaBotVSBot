using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_lizaBotVSBot
{
    class MinMaxBot : Bot
    {
        int maxDeapth = 0;
        private List<Point> pointEmptyList;
        private int currentPlayer;
        class Move
        {
            public Point move;
            public int[,] Board;
            public int Player;
            public int score = 0;
            public Move()
            {
            }
            public Move(int[,] board, Point move, int Player)
            {
                this.Player = Player;
                this.Board = board.Clone() as int[,];
                this.move = move;
                Board[move.X, move.Y] = Player;
            }
        }
        public MinMaxBot()
        {
        }
        public MinMaxBot(int maxDeapth)
        {

        }
        override public Point CalculateMove(int[,] Board, int Player)
        {
            this.currentPlayer = Player;
            pointEmptyList = pointsEmpty(Board);
            if (maxDeapth == 0)
            {
                maxDeapth = (9 * 9 / Board.Length);
                if (maxDeapth < 1)
                {
                    maxDeapth = 1;
                }
            }
            return minMax(Board, Player, maxDeapth).move;
        }
        private Move minMax(int[,] Board, int Player, int maxDeapth, int deapth = 0)
        {
            Move tempMove;
            List<Move> moves = new List<Move>();
            int score = 0;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j] == 3)
                    {
                        moves.Add(new Move(Board, new Point(i, j), Player));
                    }
                }
            }
            foreach (Move move in moves)
            {
                move.score = calculateScore(move, deapth);
                score += move.score;
                if (canPlay(move.Board) && move.score == 0)
                {
                    if (deapth < maxDeapth)
                    {
                        tempMove = minMax(move.Board, nextTurn(Player), maxDeapth, deapth + 1);
                        if (tempMove.Player == Player)
                        {
                            move.score = tempMove.score;
                        }
                        else
                        {
                            move.score = -tempMove.score;
                        }

                    }
                    else
                    {
                        move.score = CloseToWinScore(move, move.Player) - CloseToWinScore(move, nextTurn(move.Player));
                    }
                }

            }
            Move bestMove = BestMove(moves);
            bestMove.score += score;
            return bestMove;
        }
        private Move BestMove(List<Move> moves)
        {
            Random rnd = new Random();
            Move bestmove = new Move();
            bool first = true;
            bool allequal = true;
            foreach (Move m in moves)
            {
                if (first)
                {
                    bestmove = m;
                    first = false;
                }
                else
                {
                    if (bestmove.score < m.score)
                    {
                        bestmove = m;
                        allequal = false;
                    }
                    else if (bestmove.score > m.score)
                    {
                        allequal = false;
                    }
                }
            }
            if (allequal)
            {
                bestmove = moves.ElementAt(rnd.Next(moves.Count));
            }
            return bestmove;
        }
        private int nextTurn(int Player)
        {
            if (Player == 0)
            {
                Player = 1;
            }
            else
            {
                Player = 0;
            }
            return Player;
        }
        private bool canPlay(int[,] Board)
        {
            foreach (int i in Board)
            {
                if (i == 3)
                {
                    return true;
                }
            }
            return false;
        }
        private List<Point> pointsEmpty(int[,] Board)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {


                    if (Board[i, j] == 3)
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }
            return points;
        }
        private int movesAvailable(int[,] Board)
        {
            int moves = 0;
            foreach (int i in Board)
            {
                if (i == 3)
                {
                    moves++;
                }
            }
            return moves;
        }
        private int calculateScore(Move move, int deapth)
        {
            int[,] Board = move.Board;
            Point m = move.move;

            int rank = Board.GetLength(0) * Board.GetLength(1) - deapth;
            if (checkWin(move))
            {
                return 1 * rank;
            }
            else
            {
                return 0;
            }
        }
        private int CloseToWinScore(Move move, int Player)
        {
            int[,] boardInt = move.Board;
            int x = move.move.X, y = move.move.Y;
            int[] Win = { 0, 0, 0, 0 };
            for (int i = 0; i < boardInt.GetLength(0); i++)
            {
                if (boardInt[i, y] == Player)
                {
                    Win[0]++;
                }
                if (boardInt[x, i] == Player)
                {
                    Win[1]++;
                }
                if (x == y)
                {
                    if (boardInt[i, i] == Player)
                    {
                        Win[2]++;
                    }
                    if (boardInt[boardInt.GetLength(0) - 1 - i, i] == Player)
                    {
                        Win[3]++;
                    }
                }
            }
            for (int i = 0; i < boardInt.GetLength(0); i++)
            {
                if (boardInt[i, y] == nextTurn(Player))
                {
                    Win[0] = 0;
                }
                if (boardInt[x, i] == nextTurn(Player))
                {
                    Win[1] = 0;
                }
                if (x == y)
                {
                    if (boardInt[i, i] == nextTurn(Player))
                    {
                        Win[2] = 0;
                    }
                    if (boardInt[boardInt.GetLength(0) - 1 - i, i] == nextTurn(Player))
                    {
                        Win[3] = 0;
                    }
                }
            }
            return (Win[0] + Win[1] + Win[2] + Win[3]) / (boardInt.GetLength(0) * boardInt.GetLength(1));
        }
        private bool checkWin(Move move)
        {
            int[,] boardInt = move.Board;
            int x = move.move.X, y = move.move.Y;
            int Player = move.Player;
            bool[] Win = { true, true, true, true };
            for (int i = 0; i < boardInt.GetLength(0); i++)
            {
                if (boardInt[i, y] != Player)
                {
                    Win[0] = false;
                }
                if (boardInt[x, i] != Player)
                {
                    Win[1] = false;
                }
                if (boardInt[i, i] != Player)
                {
                    Win[2] = false;
                }
                if (boardInt[boardInt.GetLength(0) - 1 - i, i] != Player)
                {
                    Win[3] = false;
                }

            }

            return Win.Contains(true);
        }
    }
}
