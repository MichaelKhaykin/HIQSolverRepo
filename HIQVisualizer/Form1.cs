using HIQLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace HIQVisualizer
{
    public partial class Form1 : Form
    {

        Graphics gfx;
        Bitmap map;

        List<bool?[,]> games = new List<bool?[,]>();
        Cell[,] cells;
        int currentIndex = 0;

        int offsetX = 60;
        int offsetY = 60;

        int size = 30;
        public Form1()
        {
            InitializeComponent();

            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Width = ClientSize.Width;
            pictureBox1.Height = ClientSize.Height;

            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(map);

        }
        private void Visualize()
        {
            gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));

            var curr = games[currentIndex];

            for(int i = 0; i < curr.GetLength(0); i++)
            {
                for (int j = 0; j < curr.GetLength(1); j++)
                {
                    cells[i, j].Init(cells[i, j].CurrentColor, ArrayToColor(curr[i, j]), 0.1, 0.1);
                }
            }
        }

        private Color ArrayToColor(bool? item)
        {
            if (item == null)
            {
                return Color.Yellow;
            }
            if (item == false)
            {
                return Color.White;
            }
            return Color.Red;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        { 
            gfx.FillRectangle(Brushes.Black, 0, 0, pictureBox1.Width, pictureBox1.Height);

            Stack<Game> states = Solver.Solve();
            
            while(states.Count > 0)
            {
                games.Add(states.Pop().Value);
            }

            
            var width = games[0].GetLength(1);
            var height = games[0].GetLength(0);

            cells = new Cell[height, width];

            for (int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    cells[i, j] = new Cell(new Point(j * size + offsetX, i * size + offsetY), new Size(size - 2, size - 2), ArrayToColor(games[0][i, j]));
                }
            }
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = games.Count - 1;
            }

            Visualize();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            currentIndex++;

            if(currentIndex >= games.Count)
            {
                currentIndex = 0;
            }

            Visualize();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach(var item in cells)
            {
                item.Update();
                item.Draw(gfx);
            }

            pictureBox1.Image = map;
        }
    }
}
