using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ball
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer1;

        const int FORM_W = 1200;
        const int FORM_H = 800;
        
        private Graphics backGraphics;//Manually Managing Buffered Graphics
        private Bitmap backBmp;


        bool w, a, s, d;
        bool mouse;
        bool gameover;

        GameClass gc;
        
        const float PLAYER_SPEED = 5f;
        const float PLAYER_R = 15;
        
        const int MONSTER_MAX = 10;
        float MOVE_SPEED = 3.3f;
        const float MONSTER_R = 18;
        float MONSTER_CD = 30; // 10/30
        int cd = 0;

        
        const int BULLET_MAX = 10;
        const float BULLET_SPEED = 10f; //10f
        const float BULLET_R = 10;
        //if speed == 100 BUG is Penetrate the monster
        float lifesec = 0;

        GameObj mou;//滑鼠
        GameObj mou2;

        Random ran = new Random();

        public Form1()
        {            
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            backBmp = new Bitmap(FORM_W, FORM_H);
            backGraphics = Graphics.FromImage(backBmp); //FromImage(Image):把指定的Image做為畫布
            //this.DoubleBuffered = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_tick);
            gameInitialize();
        }

        public void gameInitialize()
        {
            w = false;a = false;
            s = false;d = false;

            MOVE_SPEED = 3.3f;
            MONSTER_CD = 30;

            timer1.Interval = 1000 / 60;
            timer1.Enabled = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Size = new Size(FORM_W, FORM_H);
      
            gameover = false;
            //ran = new Random();

            gc = new GameClass(backGraphics, FORM_W, FORM_H);
            
            gc.CraetePlayer(ran.Next(0, FORM_W), ran.Next(0, FORM_H), PLAYER_R);

            //gc.CraeteMonster(ran.Next(0, FORM_W), ran.Next(0, FORM_H), PLAYER_R);
            gc.CreateMonster(MONSTER_R, MONSTER_MAX);
        }


        private void timer1_tick(object sender, EventArgs e)
        {
            update();

            draw();

            if (gameover)
            {
                timer1.Enabled = false;
                
                MessageBox.Show("Press F5 to restart\r\n你生存了"+lifesec.ToString("#.##")+ "秒");
            }
        }

        private void update()
        {
            lifesec += (1f / 60f);
            //player
            if (w) gc.player.y -= PLAYER_SPEED; if (s) gc.player.y += PLAYER_SPEED;
            if (a) gc.player.x -= PLAYER_SPEED; if (d) gc.player.x += PLAYER_SPEED;

            //monster

            if (cd < MONSTER_CD)
            {
                cd++;
            }
            else if (gc.monst_list.Count < MONSTER_MAX) 
            {
                gc.CraeteMonster(ran.Next(0, FORM_W), ran.Next(0, FORM_H), MONSTER_R);
                if (MONSTER_CD > 0)
                    MONSTER_CD -= 0.2f;
                MOVE_SPEED += 0.05f;
                cd = 0;
            }
            gc.MoveMonster(gc.player,MOVE_SPEED);


            //bullet
            gc.MoveBullet(BULLET_SPEED);
            //collision cheak
            gc.Check();

            if (gc.player.hp <= 0) gameover = true;
        }

        private void draw()
        {
            //Clear background
            backGraphics.FillRectangle(new SolidBrush(Color.FromArgb(80, 80, 80)), 0, 0, FORM_W, FORM_H);

            Pen p = new Pen(Color.FromArgb(50,200,200,200), 2);
            for (int i = 0; i < 2000; i += 50)
            {
                backGraphics.DrawLine(p, i, 0, i, 2000);
                backGraphics.DrawLine(p, 0, i, 2000, i);
            }

            
            

            for (int i = 0; i < gc.bullet_list.Count; i++)//draw bullet
            {
                gc.bullet_list[i].darwObj(backGraphics, Color.LightYellow, BULLET_R);
            }

            for (int i = 0; i < gc.monst_list.Count; i++)//draw monster
            {
                if (gc.monst_list[i] != null)
                {
                    gc.monst_list[i].darwObj(backGraphics, Color.OrangeRed, MONSTER_R);
                    gc.monst_list[i].drawStatus(backGraphics, 20, 5);
                }
            }
            //drawplayer

            if (mou2 != null)
            {
                float d = gc.player.getLength(mou2);
                float dx = (mou2.x - gc.player.x) / d * 30;
                float dy = (mou2.y - gc.player.y) / d * 30;
                Pen p1 = new Pen(Color.PowderBlue, 15);
                backGraphics.DrawLine(p1, gc.player.x, gc.player.y, dx + gc.player.x, dy + gc.player.y);
            }

            gc.player.darwObj(backGraphics, Color.LightBlue, PLAYER_R);
            gc.player.drawStatus(backGraphics, 20, 5);
            //------------------

            string str = string.Format("子彈數量:{0}\r\n敵人數量:{1}\r\n敵人速度{2}\r\n生成速度{3}", 
                gc.bullet_list.Count.ToString(), gc.monst_list.Count.ToString(), MOVE_SPEED,MONSTER_CD);

            backGraphics.DrawString(str, DefaultFont, Brushes.Yellow, 5, 5);

            //this.Invalidate();
            //Background covered
            this.CreateGraphics().DrawImageUnscaled(this.backBmp, 0, 0);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //fire bullet
            mou = new GameObj();
            mou.x = e.X;
            mou.y = e.Y;
            gc.CraeteBullet(gc.player, mou, BULLET_R);

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mou2 = new GameObj();
            mou2.x = e.X;
            mou2.y = e.Y;
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            move(e.KeyCode, true);
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            move(e.KeyCode, false);
        }
        
        private void move(Keys k,bool t)
        {
            switch (k)
            {
                case Keys.W:
                    w = t;
                    break;
                case Keys.S:
                    s = t;
                    break;
                case Keys.A:
                    a = t;
                    break;
                case Keys.D:
                    d = t;
                    break;
                case Keys.F5:
                    gameInitialize();
                    break;
                case Keys.F1:
                    timer1.Stop();
                    break;
                case Keys.F2:
                    timer1.Start();
                    break;
            }
        }
    }
    


}
