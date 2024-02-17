using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace snakeWF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
        }

        static string applePicture = "C:\\Users\\Aleksandr\\source\\repos\\snakeWF\\snakeWF\\bin\\Debug\\resources\\apple.png";
        static string snakePicture = "C:\\Users\\Aleksandr\\source\\repos\\snakeWF\\snakeWF\\bin\\Debug\\resources\\snake.png";
        static string backgroundPicture = "C:\\Users\\Aleksandr\\source\\repos\\snakeWF\\snakeWF\\bin\\Debug\\resources\\background.png";

        class PlayGround
        {
            public PlayGround(Control.ControlCollection controlCollection)
            {
                fillarrPB(controlCollection);

                snake.Add(new SnakePart(2, 2));

                setHead(snake[0]);
            }

            public PictureBox[,] arrPB = new PictureBox[10, 10];

            private void fillarrPB(Control.ControlCollection controlCollection)
            {
                for (int i = 0; i < arrPB.GetLength(0); i++)
                {
                    for (int j = 0; j < arrPB.GetLength(1); j++)
                    {
                        arrPB[i, j] = new PictureBox();

                        arrPB[i, j].Image = new Bitmap(Image.FromFile(backgroundPicture), new Size(60, 60));

                        arrPB[i, j].BackColor = Color.White;

                        arrPB[i, j].Size = new Size(60, 60);

                        controlCollection.Add(arrPB[i, j]);
                    }
                }
            }
            public class SnakePart
            {
                public SnakePart(int Y, int X)
                {
                    this.X = X;
                    this.Y = Y;

                    prevX = X;
                    prevY = Y;
                }
                public int X { get; set; }
                public int Y { get; set; }
                public int prevX { get; set; }
                public int prevY { get; set; }
            }

            public List<SnakePart> snake = new List<SnakePart>();

            public void showarrPB()
            {
                int X = 0;
                int Y = 0;
                for (int i = 0; i < arrPB.GetLength(0); i++)
                {
                    for (int j = 0; j < arrPB.GetLength(1); j++)
                    {
                        arrPB[i, j].Location = new Point(X, Y);
                        X += 60;
                    }
                    X = 0;
                    Y += 60;
                }
            }
            public void addSnake(SnakePart snakePart)
            {
                pause = true;

                snake.Add(new SnakePart(snakePart.prevY, snakePart.prevX));

                pause = false;
            }

            private bool IsContact(SnakePart snakePart)
            {
                for (int i = 1; i < snake.Count; i++)
                {
                    if (snake[i].X == snakePart.X && snake[i].Y == snakePart.Y)
                    {
                        return true;
                    }
                }
                return false;
            }

            private void deleteHead(SnakePart snakePart)
            {
                arrPB[snakePart.Y, snakePart.X].BackColor = Color.White;

                arrPB[snakePart.Y, snakePart.X].Image = new Bitmap(Image.FromFile(backgroundPicture), new Size(60, 60));

            }
            private void setHead(SnakePart snakePart)
            {
                IsApple(snakePart);

                arrPB[snakePart.Y, snakePart.X].BackColor = Color.Red;

                arrPB[snakePart.Y, snakePart.X].Image = new Bitmap(Image.FromFile(snakePicture), new Size(60, 60));
            }

            public void nextMove(string direction, SnakePart snakePart)
            {

                switch (direction)
                {
                    case "left":
                        {
                            if (snakePart.X == 0)
                            {
                                loose = true;
                            }
                            else
                            {
                                snakePart.X--;
                            }
                            break;
                        }
                    case "right":
                        {
                            if (snakePart.X == arrPB.GetLength(0) - 1)
                            {
                                loose = true;
                            }
                            else
                            {
                                snakePart.X++;
                            }
                            break;
                        }
                    case "up":
                        {
                            if (snakePart.Y == 0)
                            {
                                loose = true;
                            }
                            else
                            {
                                snakePart.Y--;
                            }
                            break;
                        }
                    case "down":
                        {
                            if (snakePart.Y == arrPB.GetLength(0) - 1)
                            {
                                loose = true;
                            }
                            else
                            {
                                snakePart.Y++;
                            }
                            break;
                        }
                }
                setHead(snakePart);
            }

            public void IsApple(SnakePart snakepart)
            {
                foreach (var el in apples)
                {
                    if (snakepart.X == el.appleX && snakepart.Y == el.appleY)
                    {
                        addSnake(snake[snake.Count - 1]);

                        arrPB[el.appleY, el.appleX].Image = null;

                        el.appleY = el.appleX = -1;
                    }
                }
            }
            public void snakeGoAhead(Label snakeSizeLabel)
            {
                deleteHead(snake[snake.Count - 1]);

                snake[snake.Count - 1].prevX = snake[snake.Count - 1].X;
                snake[snake.Count - 1].prevY = snake[snake.Count - 1].Y;

                if (snake.Count > 1)
                {
                    for (int i = snake.Count - 1; i > 0; i--)
                    {
                        snake[i].X = snake[i - 1].X;
                        snake[i].Y = snake[i - 1].Y;
                    }
                }
                
                nextMove(direction, snake[0]);

                if (IsContact(snake[0]))
                {
                    loose = true;
                }

                snakeSizeLabel.Text = snake.Count.ToString();
            }
        }
        static bool pause = false;
        static bool loose = false;
        static string direction = "right";
        private async void appleRandomThread(PlayGround playGround)
        {
            var rnd = new Random();

            
            while (!loose)
            {
                int time = rnd.Next(3000, 5000);

                bool IsFree = false;

                await Task.Delay(time);

                while (!IsFree)
                {
                    int appleX = rnd.Next(0, 10);
                    int appleY = rnd.Next(0, 10);

                    if (playGround.arrPB[appleY, appleX].BackColor == Color.White)
                    {
                        playGround.arrPB[appleY, appleX].BackColor = Color.DarkGreen;

                        playground.arrPB[appleY, appleX].Image = new Bitmap(Image.FromFile(applePicture), new Size (60, 60));

                        apples.Add(new appleList(appleY, appleX));

                        IsFree = true;
                    }
                }
            }
        }
        private async void MoveThread(PlayGround playGround)
        {
            while (!loose)
            {
                if (!pause)
                {
                    playGround.snakeGoAhead(snakeSizeLabel);

                    playGround.showarrPB();

                    await Task.Delay(400);
                }
            }
            if (playGround.snake.Count == 100)
            {
                MessageBox.Show("you won!");
            }
            else
            {
                MessageBox.Show("you lose game over");
            }
        }

        private PlayGround playground;
        private static List<appleList> apples = new List<appleList>();

        private void Form1_Load(object sender, EventArgs e)
        {
           playground = new PlayGround(this.Controls);

            playground.showarrPB();

            new Thread(async () => MoveThread(playground)).Start();

            new Thread(async () => appleRandomThread(playground)).Start();

            KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    if (direction != "down")
                    direction = "up";
                    break;
                case Keys.S:
                    if (direction != "up")
                        direction = "down";
                    break;
                case Keys.A:
                    if (direction != "right")
                        direction = "left";
                    break;
                case Keys.D:
                    if (direction != "left")
                        direction = "right";
                    break;
            }
        }
    }
}
