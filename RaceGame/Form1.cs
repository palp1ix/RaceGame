using System;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Forms;
using RaceGame.Properties;

namespace RaceGame
{
    public partial class Form1 : Form
    {
        int speed = 1;
        int speedRotate = 5;
        int maxSpeed = 7;
        int brakePower = 5;
        int randomValue1 = 0;
        int randomValue2 = 0;
        int randomSpawn = -95;
        private int Coins = 0;
        bool IsFirstStart = true;
        Point player_car_position;
        Point enemy_car_position;
        Point enemy_car2_position;
        // Массив, в который будут сохранены все линии разметки по середине дороги
        PictureBox[] roadLines = new PictureBox[5];

        public Form1()
        {
            IsFirstStart = Convert.ToBoolean(Settings.Default["IsFirstStart"]);
            if (IsFirstStart)
            {
                MessageOnLoad();
                IsFirstStart = false;
            }
            InitializeComponent();
            label1.BringToFront();
            restart_game.BringToFront();
            // ФОНОВЫЙ МУЗОНЧИК
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream stream = assembly.GetManifestResourceStream(@"RaceGame.background.wav");
            System.Media.SoundPlayer musicPlayer = new System.Media.SoundPlayer(stream);
            musicPlayer.PlayLooping();
            //

            SetupUI();
            SaveCarsPosition();
        }

        private void SaveSettings()
        {
            //Сохранения кол-ва денег
            Settings.Default["CoinNum"] = Coins;
            //Сохранение позиции 1-го врага
            Settings.Default["EnemyCarPos_X"] = enemy_car.Location.X;
            Settings.Default["EnemyCarPos_Y"] = enemy_car.Location.Y;
            //Сохранение позиции 2-го врага
            Settings.Default["EnemyCar2Pos_Y"] = enemy_car2.Location.Y;
            Settings.Default["EnemyCar2Pos_X"] = enemy_car2.Location.X;
            //Сохранение позиции игрока
            Settings.Default["playerPos_X"] = player_car.Location.X;
            Settings.Default["playerPos_Y"] = player_car.Location.Y;
            //Сохранение ?первый запуск или нет
            Settings.Default["IsFirstStart"] = IsFirstStart;


            Settings.Default.Save();
        }

        private void LoadSettings()
        {
            Coins = Convert.ToInt32(Settings.Default["CoinNum"]);
            player_car.Top = Convert.ToInt32(Settings.Default["playerPos_Y"]);
            player_car.Left = Convert.ToInt32(Settings.Default["playerPos_X"]);
            enemy_car.Top = Convert.ToInt32(Settings.Default["EnemyCarPos_Y"]);
            enemy_car.Left = Convert.ToInt32(Settings.Default["EnemyCarPos_X"]);
            enemy_car2.Top = Convert.ToInt32(Settings.Default["EnemyCar2Pos_Y"]);
            enemy_car2.Left = Convert.ToInt32(Settings.Default["EnemyCar2Pos_X"]);
        }

        private void SaveCarsPosition()
        {
            player_car_position = player_car.Location;
            enemy_car_position = enemy_car.Location;
            enemy_car2_position = enemy_car2.Location;
        }

        //Каждый божий кадр
        private void frame1_Tick(object sender, EventArgs e)
        {
            label2.Text = (speed * 12.2).ToString();
            MoveLine(speed);
            Enemy_Controller(speed - 5);
            CoinRespawn();
            CoinPicker();
            GameOver();
        }

        //Обработка подбора монеток
        private void CoinPicker()
        {
            if (player_car.Bounds.IntersectsWith(coin1.Bounds))
            {
                Coins++;
                coin1.Top = 590;
            }
            else if (player_car.Bounds.IntersectsWith(coin2.Bounds))
            {
                Coins++;
                coin2.Top = 590;
            }
            else if (player_car.Bounds.IntersectsWith(coin3.Bounds))
            {
                Coins++;
                coin3.Top = 590;
            }
            else if (player_car.Bounds.IntersectsWith(coin4.Bounds))
            {
                Coins++;
                coin4.Top = 590;
            }
            coin_label.Text = Coins.ToString();
        }

        private void GameOver()
        {

            if (player_car.Bounds.IntersectsWith(enemy_car.Bounds) || player_car.Bounds.IntersectsWith(enemy_car2.Bounds))
            {
                frame1.Enabled = false;
                speed_down.Enabled = false;
                label1.Visible = true;
                restart_game.Visible = true;
                restart_game.Enabled = true;
            }
        }

        // Управление врагами
        private void Enemy_Controller(int speed)
        {
            Random r = new Random();
            if (enemy_car.Top >= 590)
            {
                enemy_car.Image = enemy_car.Image.VerticalResolution == Resource1.silvia.VerticalResolution ? Resource1.golf : Resource1.silvia;
                randomSpawn = r.Next(-200, -95);
                enemy_car.Top = randomSpawn;
                enemy_car.Left = r.Next(221, 366);
                randomValue1 = r.Next(2, 15);
            }
            else
                enemy_car.Top += speed + randomValue1;

            if (enemy_car2.Top >= 590)
            {
                enemy_car2.Image = enemy_car2.Image.VerticalResolution == Resource1.silvia.VerticalResolution ? Resource1.golf : Resource1.silvia;
                randomSpawn = r.Next(-200, -95);
                enemy_car2.Left = r.Next(3, 157);
                enemy_car2.Top = randomSpawn;
                randomValue2 = r.Next(2, 15);
            }
            else
                enemy_car2.Top += speed + randomValue2;
        }

        // По названию можно понять
        private void CoinRespawn()
        {
            Random r = new Random();
            if (coin1.Top >= 590)
            {
                coin1.Top = r.Next(-200, -95);
                coin1.Left = r.Next(17, 190);
            }
            else
            {
                coin1.Top += speed;
            }

            if (coin2.Top >= 590)
            {
                coin2.Top = r.Next(-200, -95);
                coin2.Left = r.Next(17, 190);
            }
            else
            {
                coin2.Top += speed;
            }

            if (coin3.Top >= 590)
            {
                coin3.Top = r.Next(-200, -95);
                coin3.Left = r.Next(230, 366);
            }
            else
            {
                coin3.Top += speed;
            }

            if (coin4.Top >= 590)
            {
                coin4.Top = r.Next(-200, -95);
                coin4.Left = r.Next(230, 366);
            }
            else
            {
                coin4.Top += speed;
            }
        }

        //Движение полос
        private void MoveLine(int speed)
        {
            if (pictureBox1.Top >= 590)
                pictureBox1.Top = -95;
            else
                pictureBox1.Top += speed;

            if (pictureBox2.Top >= 590)
                pictureBox2.Top = -95;
            else
                pictureBox2.Top += speed;

            if (pictureBox3.Top >= 590)
                pictureBox3.Top = -95;
            else
                pictureBox3.Top += speed;
            if (pictureBox4.Top >= 590)
                pictureBox4.Top = -95;
            else
                pictureBox4.Top += speed;
            if (pictureBox5.Top >= 590)
                pictureBox5.Top = -95;
            else
                pictureBox5.Top += speed;
        }

        private void roadLinesSync()
        {
            roadLines[0].Top = -100;
            for (int j = 1; j < roadLines.Length; j++)
            {
                roadLines[j].Top = roadLines[j - 1].Top + 138;
            }
        }

        //Управление машиной
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Если игра закончилась, то ничего не делать
            if (label1.Visible)
                return;
            else if (label7.Visible)
            {
                frame1.Enabled = true;
                speed_down.Enabled = true;
                label7.Visible = false;
                return;
            }

            switch (e.KeyCode)
            {
                //Поворот на право
                case Keys.D:
                    // Если у стенки, то поворачивать нельзя
                    if (player_car.Location.X >= 375)
                    {
                        
                    }
                    else
                        player_car.Left += speedRotate;
                    break;
                //Поворот на лево
                case Keys.A:
                    // Если у стенки, то поворачивать нельзя
                    if (player_car.Location.X <= 20)
                    {
                        //ПУСТО
                    }
                    else
                        player_car.Left -= speedRotate;
                    break;
                //Ускорение
                case Keys.W:
                    if (player_car.Location.Y <= 0 && speed >= maxSpeed)
                    {
                        //ПУСТО
                    }
                    else if (player_car.Location.Y >= 0 && speed >= maxSpeed)
                        player_car.Top += -2;
                    else
                    {
                        player_car.Top += -2;
                        speed++;
                    }
                    break;
                //Торможение
                case Keys.S:

                    if (player_car.Location.Y >= 450 && speed <= 0)
                        return;
                    else if (player_car.Location.Y <= 450 && speed <= 4)
                        player_car.Top += 2;
                    else
                    {
                        player_car.Top += brakePower;
                        speed--;
                    }
                    break;
            }
        }


        private void speed_down_Tick(object sender, EventArgs e)
        {
            if (speed > 0)
                speed--;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            speed = 7;
            label1.Visible = false;
            restart_game.Enabled = false;
            restart_game.Visible = false;
            speed_down.Enabled = true;
            frame1.Enabled = true;
            roadLinesSync();
            ResetPosition();
        }

        private void ResetPosition()
        {
            player_car.Location = player_car_position;
            enemy_car.Location = enemy_car_position;
            enemy_car2.Location = enemy_car2_position;
            this.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Settings.Default["EnemyCarPos_X"]) != 0)
                LoadSettings();
            for (int i = 0; i <= 5; i++)
            {
                // Используем метод Find(), чтобы найти PictureBox с именем "pictureBox" и номером i
                PictureBox pictureBox = Controls.Find("pictureBox" + i.ToString(), true).FirstOrDefault() as PictureBox;

                // Если PictureBox найден, сохраняем его в массив
                if (pictureBox != null)
                    roadLines[i - 1] = pictureBox;
            }

            for (int j = 1; j < roadLines.Length; j++)
            {
                roadLines[j].Top = roadLines[j - 1].Top + 138;
            }
        }

        private void MessageOnLoad()
        {
            MessageBox.Show("Приветствую! Игра - аркадные гонки. Правила просты: гони быстрее, собирай баксы. Удачи!", "Правила", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SetupUI()
        {
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Font = new System.Drawing.Font("Courier New", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(55, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(526, 73);
            this.label1.TabIndex = 1;
            this.label1.Text = "Игра окончена";
            this.label1.Visible = false;
            // 
            // restart_game
            // 
            this.restart_game.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.restart_game.Enabled = false;
            this.restart_game.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.restart_game.Location = new System.Drawing.Point(241, 276);
            this.restart_game.Name = "restart_game";
            this.restart_game.Size = new System.Drawing.Size(151, 49);
            this.restart_game.TabIndex = 3;
            this.restart_game.Text = "Заново";
            this.restart_game.UseVisualStyleBackColor = false;
            this.restart_game.Visible = false;
            this.restart_game.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(65, 244);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(507, 25);
            this.label7.TabIndex = 12;
            this.label7.Visible = false;
            this.label7.Text = "Нажмите любую клавишу для продолжения...";
            this.label7.BackColor = System.Drawing.SystemColors.ActiveCaption;
        }

        private void pictureBox1_LocationChanged(object sender, EventArgs e)
        {
            if(pictureBox1.Location.Y == -100)
            {
                for (int i = 1; i <= roadLines.Length - 1; i++)
                {
                    roadLines[i].Top = roadLines[i - 1].Top + 138;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void Pause()
        {
            frame1.Enabled = false;
            speed_down.Enabled = false;
            label7.Visible = true;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (!label1.Visible)
                Pause();
        }
    }
}
