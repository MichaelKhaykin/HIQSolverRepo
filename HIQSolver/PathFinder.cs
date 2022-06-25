using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HIQSolver
{
    public static class PathFinder<T> where T : Game
    {
        public static Stack<Game> FindPath(T start, Func<int, int, int> Huerisitic, Func<Game, List<Game>> GenerateNeighbors)
        {
            start.GScore = 0;
            //known starting moves is 4 from original point in game
            start.FScore = Huerisitic(start.AmountOfPegs, 4);

            Game end = null;

            PriorityQueue<Game> queue = new PriorityQueue<Game>(10000);
            queue.Insert(start);

            List<bool?[,]> seen = new List<bool?[,]>();

            while (queue.Count > 0)
            {
                var deq = queue.Pop();

                var neighbors = GenerateNeighbors(deq);
                if (neighbors.Count == 0 && deq.AmountOfPegs == 1)
                {
                    end = deq;
                    break;
                }

                for (int i = 0; i < neighbors.Count; i++)
                {
                    var board = neighbors[i].Value;

                    bool total = true;
                    foreach (var array in seen)
                    {
                        bool good = false;
                        for(int x = 0; x < board.GetLength(0); x++)
                        {
                            for(int j = 0; j < board.GetLength(1); j++)
                            {
                                if(board[x, j] != array[x, j])
                                {
                                    good = true;
                                    x = 100;
                                    break;
                                }
                            }
                        }

                        if(!good)
                        {
                            total = false;
                            break;
                        }
                    }

                    if (total == false) continue;
                
                    var tentativeDistance = deq.GScore + neighbors[i].Distance;
                    if (tentativeDistance < neighbors[i].GScore)
                    {
                        neighbors[i].GScore = tentativeDistance;
                        neighbors[i].Founder = deq;
                        neighbors[i].FScore = neighbors[i].GScore + Huerisitic(neighbors[i].AmountOfPegs, neighbors.Count);
                    }

                    queue.Insert(neighbors[i]);
                    seen.Add(neighbors[i].Value);
                }
            }

            Stack<Game> gameStack = new Stack<Game>();
            while (end != null)
            {
                gameStack.Push(end);
                end = end.Founder;
            }

            return gameStack;
        }
    }
}
