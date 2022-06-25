using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQLib
{
    public static class PathFinder<T> where T : Game
    {
        public static Stack<Game> FindPath(T start, Func<int, int, int> Huerisitic, Func<Game, List<Game>> GenerateNeighbors)
        {
            start.GScore = 0;
            start.FScore = Huerisitic(start.AmountOfPegs, 4);

            Game end = null;

            PriorityQueue<Game> queue = new PriorityQueue<Game>(10000);
            queue.Insert(start);

            while (queue.Count > 0)
            {
                var deq = queue.Pop();

                var neighbors = GenerateNeighbors(deq);
                if (neighbors.Count == 0 && deq.AmountOfPegs == 1 && deq.Value[deq.Value.GetLength(0) / 2, deq.Value.GetLength(1) / 2] == true)
                {
                    end = deq;
                    break;
                }

                for (int i = 0; i < neighbors.Count; i++)
                { 
                    var tentativeDistance = deq.GScore + neighbors[i].Distance;
                    if (tentativeDistance < neighbors[i].GScore)
                    {
                        neighbors[i].GScore = tentativeDistance;
                        neighbors[i].Founder = deq;
                        neighbors[i].FScore = neighbors[i].GScore + Huerisitic(neighbors[i].AmountOfPegs, neighbors.Count);
                    }

                    queue.Insert(neighbors[i]);
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
