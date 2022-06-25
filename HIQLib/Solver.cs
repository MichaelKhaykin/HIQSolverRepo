using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQLib
{
    public static class Solver
    {
        public static Stack<Game> Solve()
        {
            #region SetUpBoard
            bool?[,] mainBoard = new bool?[7, 7];

            int wallSize = 2;

            for (int i = 0; i < mainBoard.GetLength(0); i++)
            {
                for (int j = 0; j < mainBoard.GetLength(1); j++)
                {
                    if (i < wallSize && j < wallSize) continue;
                    if (j > mainBoard.GetLength(1) - wallSize - 1 && i < wallSize) continue;
                    if (i > mainBoard.GetLength(0) - wallSize - 1 && j > mainBoard.GetLength(1) - wallSize - 1) continue;
                    if (i > mainBoard.GetLength(0) - wallSize - 1 && j < wallSize) continue;

                    mainBoard[i, j] = true;
                }
            }
            mainBoard[mainBoard.GetLength(0) / 2, mainBoard.GetLength(1) / 2] = false;
            #endregion

            Game start = new Game(mainBoard, 32);

            Func<int, int, int> Hueristic = (n, c) =>
            {
                return n * 100 - c;
            };

            Func<Game, List<Game>> Neighbors = (g) =>
            {
                List<Game> newGames = new List<Game>();

                var board = g.Value;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == null || board[i, j] == false) continue;

                        if (j - 2 >= 0 && board[i, j - 1] == true && board[i, j - 2] == false)
                        {
                            Game newGame = new Game(board, g.AmountOfPegs - 1);
                            newGame.Value[i, j - 2] = true;
                            newGame.Value[i, j - 1] = false;
                            newGame.Value[i, j] = false;

                            newGames.Add(newGame);
                        }
                        if (j + 2 < board.GetLength(1) && board[i, j + 1] == true && board[i, j + 2] == false)
                        {
                            Game newGame = new Game(board, g.AmountOfPegs - 1);
                            newGame.Value[i, j + 2] = true;
                            newGame.Value[i, j + 1] = false;
                            newGame.Value[i, j] = false;

                            newGames.Add(newGame);
                        }
                        if (i - 2 >= 0 && board[i - 1, j] == true && board[i - 2, j] == false)
                        {
                            Game newGame = new Game(board, g.AmountOfPegs - 1);
                            newGame.Value[i - 2, j] = true;
                            newGame.Value[i - 1, j] = false;
                            newGame.Value[i, j] = false;

                            newGames.Add(newGame);
                        }
                        if (i + 2 < board.GetLength(0) && board[i + 1, j] == true && board[i + 2, j] == false)
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
            return endGame;
        }
    }
}
