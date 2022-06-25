using System;
using System.Collections.Generic;
using System.Drawing;

namespace HIQSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();

            PriorityQueue<int> p = new PriorityQueue<int>(10);
            for(int i = 0; i < 10; i++)
            {
                p.Insert(rand.Next(0, 100));
            }

            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(p.Pop());
            }

            #region SetUpBoard
            bool?[,] board = new bool?[7, 7];

            int wallSize = 2;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (i < wallSize && j < wallSize) continue;
                    if (j > board.GetLength(1) - wallSize - 1 && i < wallSize) continue;
                    if (i > board.GetLength(0) - wallSize - 1 && j > board.GetLength(1) - wallSize - 1) continue;
                    if (i > board.GetLength(0) - wallSize - 1 && j < wallSize) continue;

                    board[i, j] = true;
                }
            }
            board[board.GetLength(0) / 2, board.GetLength(1) / 2] = false;
            #endregion

            Game start = new Game(board, 32);

            Func<int, int, int> Hueristic = (n, c) =>
            {
                return n * 5 - c;
            };

            Func<Game, List<Game>> Neighbors = (g) =>
            {
                List<Game> newGames = new List<Game>();

                var board = g.Value;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for(int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == null || board[i, j] == false) continue;

                        if(j - 2 >= 0 && board[i, j - 1] == true && board[i, j - 2] == false)
                        {
                            Game newGame = new Game(board, g.AmountOfPegs - 1);
                            newGame.Value[i, j - 2] = true;
                            newGame.Value[i, j - 1] = false;
                            newGame.Value[i, j] = false;

                            newGames.Add(newGame);
                        }
                        if(j + 2 < board.GetLength(1) && board[i, j + 1] == true && board[i, j + 2] == false)
                        {
                            Game newGame = new Game(board, g.AmountOfPegs - 1);
                            newGame.Value[i, j + 2] = true;
                            newGame.Value[i, j + 1] = false;
                            newGame.Value[i, j] = false;

                            newGames.Add(newGame);
                        }
                        if(i - 2 >= 0 && board[i - 1, j] == true && board[i - 2, j] == false)
                        {
                            Game newGame = new Game(board, g.AmountOfPegs - 1);
                            newGame.Value[i - 2, j] = true;
                            newGame.Value[i - 1, j] = false;
                            newGame.Value[i, j] = false;

                            newGames.Add(newGame);
                        }
                        if(i + 2 < board.GetLength(0) && board[i + 1, j] == true && board[i + 2, j] == false)
                        {
                            Game newGame = new Game(board, g.AmountOfPegs - 1);
                            newGame.Value[i + 2, j] = true;
                            newGame.Value[i + 1, j] = false;
                            newGame.Value[i, j] = false;

                            newGames.Add(newGame);
                        }
                    }
                }

                return newGames;
            };

            Stack<Game> endGame = PathFinder<Game>.FindPath(start, Hueristic, Neighbors);

            
        }

        public static void PrintBoard(bool?[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == null)
                    {
                        Console.Write("N");
                        Console.Write(" ");
                        continue;
                    }

                    Console.Write(board[i, j].ToString()[0]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            
        }
    }
}
