using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQVisualizer
{
    public class Cell
    {
        public Point Position { get; set; }
        public Size Size { get; set; }
        public Color CurrentColor { get; set; }
        public Rectangle HitBox
        {
            get
            {
                return new Rectangle(Position, Size);
            }
        }

        private double by;
        private double step;
        private LerpStates lerpState;
        private Color init;
        private Color final;
        public Cell(Point position, Size size, Color col)
        {
            Position = position;
            Size = size;
            lerpState = LerpStates.None;
            CurrentColor = col;
        }

        public void Init(Color init, Color goal, double by, double step)
        {
            lerpState = LerpStates.Lerping;
            this.init = init;
            this.final = goal;
            this.by = by;
            this.step = step;
        }
        private double Lerp(double a, double b, double by)
        {
            return a + (b - a) * by;
        }
        private Color LerpColor(Color a, Color b, double by)
        {
            var newR = Lerp(a.R, b.R, by);
            var newG = Lerp(a.G, b.G, by);
            var newB = Lerp(a.B, b.B, by);

            return Color.FromArgb((int)newR, (int)newG, (int)newB);
        }
        public void Update()
        {
            switch (lerpState)
            {
                case LerpStates.Lerping:
                    CurrentColor = LerpColor(init, final, by);
                    by += step;

                    if(by >= 1)
                    {
                        lerpState = LerpStates.None;
                    }

                    break;

                case LerpStates.None:
                    break;
            }
        }
        public void Draw(Graphics gfx)
        {
            gfx.FillRectangle(new SolidBrush(CurrentColor), HitBox);
        }
    }
}
