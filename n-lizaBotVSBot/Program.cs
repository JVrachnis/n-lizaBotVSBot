using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace n_lizaBotVSBot
{
    class Program
    {
        static int[,] boardInt= new int[3,3];
        static List<Point> emptyPoints = new List<Point>();
        static int Games =0;
        static int MaxGames = 300;
        static int currentPlayer = 0;
        static Bot[] bot = { new MinMaxBot(), new MinMaxBot() };
        static void Main(string[] args)
        {
            InitializeBoardArray();
            playBotIfPossible();
        }
        static private void InitializeBoardArray()
        {
            for (int i = 0; i < boardInt.GetLength(0); i++)
            {
                for (int j = 0; j < boardInt.GetLength(1); j++)
                {
                    boardInt[i, j] = 3;
                    emptyPoints.Add(new Point(i, j));
                }
            }
        }
        static private void possitionPlayed(int x, int y)
        {boardInt[x, y] = currentPlayer;
            outPutStateToCVS();
            outPutMoveToCVS(x, y, currentPlayer);
            if (checkWin(x, y))
            {
                victory();
            }
            else
            if (checkdraw())
            {
                draw();
            }
            else
            {
                nextTurn();
            }
        }
        static private void draw()
        {
            Games++;
            Console.WriteLine("draw Games: " + Games);
            InitializeBoardArray();
            if (Games < MaxGames)
            {
                nextTurn();
            }
        }
        static private bool checkdraw()
        {
            foreach (int i in boardInt)
            {
                if (i == 3)
                {
                    return false;
                }
            }
            return true;
        }
        static private void victory()
        {
            Games++;
            Console.WriteLine("victory Games: " + Games);
            InitializeBoardArray();
            if (Games<MaxGames)
            {
                nextTurn();
            }
        }
        static private void nextTurn()
        {
            if (currentPlayer == 0)
            {
                currentPlayer = 1;
            }
            else
            {
                currentPlayer = 0;
            }
            playBotIfPossible();
        }
        static private void playBotIfPossible()
        {
            Point p = bot[currentPlayer].CalculateMove(boardInt, currentPlayer);
            possitionPlayed(p.X, p.Y);

        }
        static private bool checkWin(int x,int y)
        {
            bool[] Win = { true, true, true, true };
            for (int i = 0; i < boardInt.GetLength(0); i++)
            {
                if (boardInt[i, y] != currentPlayer)
                {
                    Win[0] = false;
                }
                if (boardInt[x, i] != currentPlayer)
                {
                    Win[1] = false;
                }
                if (boardInt[i, i] != currentPlayer)
                {
                    Win[2] = false;
                }
                if (boardInt[boardInt.GetLength(0) - 1 - i, i] != currentPlayer)
                {
                    Win[3] = false;
                }

            }

            return Win.Contains(true);
        }
        static private void outPutStateToCVS()
        {
            int[] board1D = new int[boardInt.GetLength(0) * boardInt.GetLength(1)];
            for (int i = 0; i < boardInt.GetLength(0); i++)
            {
                for (int j = 0; j < boardInt.GetLength(1); j++)
                {
                    board1D[i * boardInt.GetLength(0) + j] = translateInt(boardInt[i, j]);
                }
            }
            writeToCSV(board1D, ".\\state" + boardInt.GetLength(0) + "X" + boardInt.GetLength(1) + ".csv");
        }
        static private void outPutMoveToCVS(int x, int y, int player)
        {
            int[] board1D = new int[boardInt.GetLength(0) * boardInt.GetLength(1)];
            for (int i = 0; i < boardInt.GetLength(0); i++)
            {
                for (int j = 0; j < boardInt.GetLength(1); j++)
                {
                    board1D[i * boardInt.GetLength(0) + j] = 0;
                }
            }
            board1D[x * y] = translateInt(player);
            writeToCSV(board1D, ".\\move" + boardInt.GetLength(0) + "X" + boardInt.GetLength(1) + ".csv");
        }
        static private void writeToCSV(int[] board, string path)
        {

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    writeIntToCSV(sw, board);
                }
            }
            else
            {
                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(path))
                {
                    writeIntToCSV(sw, board);
                }
            }
        }
        static private void writeIntToCSV(StreamWriter sw, int[] board)
        {
            sw.WriteLine(string.Join(",", board));
        }
        static private int translateInt(int i)
        {
            if (i == 0)
            {
                return -1;
            }
            else if (i == 3)
            {
                return 0;
            }
            return i;
        }
    }
}
