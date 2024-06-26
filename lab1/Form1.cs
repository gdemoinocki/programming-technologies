﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace TP.LAB1.Arkanoid
{
    public partial class Form1 : Form
    {
        const int mapWidth = 22;
        const int mapHeight = 25;
        public int[,] map = new int[mapHeight, mapWidth];
        //направление полета
        public int dirX = 0;
        public int dirY = 0;
        //координаты платформы
        public int platformX;
        public int platformY;
        //координаты шарика
        public int ballX;
        public int ballY;

        public int score;
        Timer timer1;
        public Image arkanoidSet;

        public Form1()
        {
            InitializeComponent();

            label1.Text = "Score: " + score;

            //label1.Text = "Score: " + score;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(Update);
            this.KeyUp += new KeyEventHandler(InputCheck);
            Init(); 
        }

        private void InputCheck(object sender, KeyEventArgs e)//Управление платформой
        {
            map[platformY, platformX] = 0;
            map[platformY, platformX + 1] = 0;
            map[platformY, platformX + 2] = 0;
            map[platformY, platformX + 3] = 0;
            map[platformY, platformX + 4] = 0;
            map[platformY, platformX + 5] = 0;
            map[platformY, platformX + 6] = 0;
            map[platformY, platformX + 7] = 0;
            int previousPlatformX = platformX;
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        if (platformX + 1 < mapWidth - 1)
                            platformX++;
                        break;
                    case Keys.Left:
                        if (platformX > 0)
                            platformX--;
                        break;
                }
                map[platformY, platformX] = 9;
                map[platformY, platformX + 1] = 99;
                map[platformY, platformX + 2] = 999;
                map[platformY, platformX + 3] = 9999;
                map[platformY, platformX + 4] = 9999;
                map[platformY, platformX + 5] = 9999;
                map[platformY, platformX + 6] = 9999;
                map[platformY, platformX + 7] = 9999;
            }
            catch (IndexOutOfRangeException ex)//Обработка ошибки IndexOutOfRangeException 
            {   
                platformX = previousPlatformX;
                map[platformY, platformX] = 9;
                map[platformY, platformX + 1] = 99;
                map[platformY, platformX + 2] = 999;
                map[platformY, platformX + 3] = 9999;
                map[platformY, platformX + 4] = 9999;
                map[platformY, platformX + 5] = 9999;
                map[platformY, platformX + 6] = 9999;
                map[platformY, platformX + 7] = 9999;
            }
        }
        public void AddLine()
        {
            for (int i = mapHeight - 2; i > 0; i--)
            {
                for (int j = 0; j < mapWidth; j += 2)
                {
                    map[i, j] = map[i - 1, j];
                }
            }
            for (int j = 0; j < mapWidth; j += 2)
            {
                map[0, j] = 1;
            }
        }
        private void Update(object sender, EventArgs e)//Движение шарика
        {
            if (ballY + dirY > mapHeight - 1)
            {  
                timer1.Stop();
                this.Hide();
                Form3 f3 = new Form3();
                f3.label3.Text = this.label1.Text;
                f3.ShowDialog();
                this.Close();
            }

            map[ballY, ballX] = 0;
            if (!IsCollide())
                ballX += dirX;
            if (!IsCollide())
                ballY += dirY;
            map[ballY, ballX] = 8;

            map[platformY, platformX] = 9;
            map[platformY, platformX + 1] = 99;
            map[platformY, platformX + 2] = 999;
            map[platformY, platformX + 3] = 9999;
            map[platformY, platformX + 4] = 9999;
            map[platformY, platformX + 5] = 9999;
            map[platformY, platformX + 6] = 9999;
            map[platformY, platformX + 7] = 9999;

            Invalidate();
        }
        public void GeneratePlatforms()//Генерация кирпичиков
        {

            for (int i = 0; i < mapHeight / 15; i++)
            {
                for (int j = 0; j < mapWidth; j +=2)
                {
                    map[i, j] = 1; 
                }
            }
        }

        public bool IsCollide()//Проверка столкновения шарика со стенками и платформой
        {
            bool isColliding = false;
            if (ballX + dirX > mapWidth - 1 || ballX + dirX < 0)
            {
                dirX *= -1;
                isColliding = true;
            }
            if (ballY + dirY > mapHeight- 1 || ballY + dirY < 0)
            {
                dirY *= -1;
                isColliding = true;
            }
            if (map[ballY + dirY, ballX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                if (map[ballY + dirY, ballX] > 10 && map[ballY + dirY, ballX] < 99)
                {
                    map[ballY + dirY, ballX] = 0;
                    map[ballY + dirY, ballX - 1] = 0;
                    addScore = true;
                }
                else if (map[ballY + dirY, ballX] < 9)
                {
                    map[ballY + dirY, ballX] = 0;
                    map[ballY + dirY, ballX + 1] = 0;
                    addScore = true;
                }
                if (addScore)
                {
                    //MessageBox.Show("igdyduy");
                    score += 50;
                    if (score % 200 == 0 && score > 0)
                    {
                        AddLine();
                    }
                }
                dirY *= -1;
            }
            if (map[ballY, ballX + dirX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                if (map[ballY, ballX + dirX] > 10 && map[ballY + dirY, ballX] < 99)
                {
                    map[ballY, ballX + dirX] = 0;
                    map[ballY, ballX + dirX - 1] = 0;
                    addScore = true;
                }
                else if (map[ballY, ballX + dirX] < 9)
                {
                    map[ballY, ballX + dirX] = 0;
                    map[ballY, ballX + dirX + 1] = 0;
                    addScore = true;
                }
                if (addScore)
                {
                    score += 50;
                    if (score % 200 == 0 && score > 0)
                    {
                        AddLine();
                    }
                }
                dirX *= -1;
            }
            label1.Text = "счет: " + score;
            return isColliding;
        }

        public void DrawArea(Graphics g)//Граница
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, mapWidth * 20, mapHeight*20 + 7));
        }

        public void Init()//Иницилизация основной информации
        {
            label1.Text = "cчет: " + score;

            arkanoidSet = new Bitmap("C:\\Users\\4769003\\source\\repos\\TP.LAB1.Arkanoid\\arkanoid.png");//спрайты
            timer1.Interval = 60;

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = 0;
                }
            }

            platformX = (mapWidth - 8) / 2;
            platformY = mapHeight - 1;
            //добавляем платформу
            map[platformY, platformX] = 9;
            map[platformY, platformX + 1] = 99;
            map[platformY, platformX + 2] = 999;
            map[platformY, platformX + 3] = 9999;
            map[platformY, platformX + 4] = 9999;
            map[platformY, platformX + 5] = 9999;
            map[platformY, platformX + 6] = 9999;
            map[platformY, platformX + 7] = 9999;
            //добавляем шарик
            ballY = platformY - 1;
            ballX = platformX + 13;
            map[ballY, ballX] = 8;

            GeneratePlatforms();

            dirX = 1;
            dirY = -1;

            timer1.Start();
        }

        public void DrawMap(Graphics g)//Отрисовка платформы
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (map[i, j] == 9)//платформа
                    {
                        g.DrawImage(arkanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(155, 35)), 568, 372, 219, 50, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 8)//шарик
                    {
                        g.DrawImage(arkanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(21, 21)), 806, 548, 73, 73, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 1)
                    {
                        g.DrawImage(arkanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16, 170, 59, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)//Метод отрисовки холста
        {
            DrawMap(e.Graphics);
            DrawArea(e.Graphics);
        }
    }
}
