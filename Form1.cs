using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{

    public partial class Form1 : Form
    {

        private Graphics graphics;
        private int resolution;
        GameEngine gameEngine;
        Brush ColorMain = Brushes.Crimson;
        Color ColorBack = Color.Black;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            nudMaxNeighbours.Enabled = false;
            nudMinNeighbours.Enabled = false;
            bPause.Enabled = true;

            PictureBox1.Image = new Bitmap(PictureBox1.Width, PictureBox1.Height);
            graphics = Graphics.FromImage(PictureBox1.Image);
            resolution = (int)nudResolution.Minimum + (int)nudResolution.Maximum - (int)nudResolution.Value;

            gameEngine = new GameEngine
            (
                cols: PictureBox1.Width / resolution,
                rows: PictureBox1.Height / resolution,
                density: (int)nudDensity.Minimum + (int)nudDensity.Maximum - (int)nudDensity.Value,
                minNeighbours_: (int)nudMinNeighbours.Value,
                maxNeighbours_: (int)nudMaxNeighbours.Value
            );

            timer1.Start();
        }

        private void DrawNextGeneration()
        {
            graphics.Clear(ColorBack);

            var field = gameEngine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                    {
                        graphics.FillRectangle(ColorMain, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }
            PictureBox1.Refresh();
            gameEngine.NextGeneration();
        }


        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            
            timer1.Stop();

            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
            nudMaxNeighbours.Enabled = true;
            nudMinNeighbours.Enabled = true;
            bPause.Enabled = false;
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.AddCell(x, y);
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.RemoveCell(x, y);
            }

        }

        private void bBackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
                ColorBack = colorDialog1.Color;
                PictureBox1.BackColor = colorDialog1.Color;
            }
                
        }

        private void bMainColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel2.BackColor = colorDialog1.Color;
                ColorMain = new SolidBrush(colorDialog1.Color);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value < 0)
            {
                timer1.Interval = 50 - trackBar1.Value * 15;
            }
            else
            {
                timer1.Interval = 50 - trackBar1.Value * 2;
            }
        }

        private void bPause_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }
    }
}
