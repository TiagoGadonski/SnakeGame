using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Point> snake = new List<Point>();
        private Point food;
        private int score = 0;
        private Timer gameTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Width = 800;
            this.Height = 600;
            this.Text = "Snake Game";

            gameTimer.Interval = 100;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            this.KeyDown += new KeyEventHandler(OnKeyPress);
            this.Paint += new PaintEventHandler(Draw);

            StartGame();
        }

        private void StartGame()
        {
            snake.Clear();
            snake.Add(new Point(10, 10));
            score = 0;
            GenerateFood();
        }

        private void GenerateFood()
        {
            Random rand = new Random();
            food = new Point(rand.Next(0, this.Width / 20), rand.Next(0, this.Height / 20));
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            MoveSnake();
            this.Invalidate(); // Redraw screen
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            // Draw snake
            foreach (Point p in snake)
            {
                canvas.FillRectangle(Brushes.Green, new Rectangle(p.X * 20, p.Y * 20, 20, 20));
            }

            // Draw food
            canvas.FillRectangle(Brushes.Red, new Rectangle(food.X * 20, food.Y * 20, 20, 20));

            // Draw score
            canvas.DrawString($"Score: {score}", new Font("Arial", 16), Brushes.Black, new PointF(10, 10));
        }

        private void MoveSnake()
        {
            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i] = snake[i - 1];
            }

            if (direction == "up")
            {
                snake[0] = new Point(snake[0].X, snake[0].Y - 1);
            }
            else if (direction == "down")
            {
                snake[0] = new Point(snake[0].X, snake[0].Y + 1);
            }
            else if (direction == "left")
            {
                snake[0] = new Point(snake[0].X - 1, snake[0].Y);
            }
            else if (direction == "right")
            {
                snake[0] = new Point(snake[0].X + 1, snake[0].Y);
            }

            // Check for collision with food
            if (snake[0] == food)
            {
                snake.Add(new Point(food.X, food.Y));
                score += 10;
                GenerateFood();
            }

            // Check for collision with borders
            if (snake[0].X < 0 || snake[0].X >= this.Width / 20 || snake[0].Y < 0 || snake[0].Y >= this.Height / 20)
            {
                GameOver();
            }

            // Check for collision with itself
            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[0] == snake[i])
                {
                    GameOver();
                }
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show($"Game Over! Your score is {score}", "Snake Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            StartGame();
            gameTimer.Start();
        }

        private string direction = "right";

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && direction != "down")
            {
                direction = "up";
            }
            else if (e.KeyCode == Keys.Down && direction != "up")
            {
                direction = "down";
            }
            else if (e.KeyCode == Keys.Left && direction != "right")
            {
                direction = "left";
            }
            else if (e.KeyCode == Keys.Right && direction != "left")
            {
                direction = "right";
            }
        }

        private Timer timer1;
        private System.ComponentModel.IContainer components;
        private Label label1;
    }
}
