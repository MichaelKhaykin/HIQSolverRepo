using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQLib
{
    public class Game : IComparable<Game>
    {
        public bool?[,] Value { get; }
        public double FScore { get; set; }
        public double GScore { get; set; }
        public Game Founder { get; set; }
        public double Distance { get; }
        public int AmountOfPegs { get; set; }
        public Game(bool?[,] board, int amountOfPegs)
        {
            AmountOfPegs = amountOfPegs;

            Value = new bool?[board.GetLength(0), board.GetLength(1)];
            Array.Copy(board, Value, board.GetLength(0) * board.GetLength(1));


            GScore = double.PositiveInfinity;
            FScore = double.PositiveInfinity;
            Founder = null;

            Distance = 1;
        }

        public int CompareTo(Game other)
        {
            return FScore.CompareTo(other.FScore);
        }
    }
}
