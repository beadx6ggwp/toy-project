using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basic_Tile_Based_Platformer
{
    public partial class Form1 : Form
    {
        public int WIDTH = 400, OffsetW = 16;
        public int HEIGHT = 400, OffsetH = 39;

        Graphics g;
        Bitmap backBmp;
        Color Backcolor = Color.White;

        private bool running;

        private Timer timer;
        private int FPS = 60;
        private int targetTime;// = 1000 / FPS

        private TileMap tilemap;
        private Player player;
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(keyDown);
            this.KeyUp += new KeyEventHandler(keyUp);

            WIDTH += OffsetW; HEIGHT += OffsetH;// offset 400x400
            this.Size = new Size(WIDTH, HEIGHT);
            this.timer = new Timer();
            this.timer.Tick += new System.EventHandler(gameloop);

            run();
        }

        public void run()
        {
            init();// initialize
            running = true;

            timer.Start();// gameloop
        }

        public void init()
        {
            backBmp = new Bitmap(Width, HEIGHT);
            g = Graphics.FromImage(backBmp);
            timer.Interval = 1000 / FPS;

            tilemap = new TileMap("map.txt",32);
            player = new Player(tilemap);
            player.setx(50);
            player.sety(50);
        }

        public void gameloop(object sendeer, EventArgs e)
        {
            update();
            render();
            draw();
        }

        public void update()
        {
            tilemap.update();
            player.update();
        }
        
        public void render()
        {
            tilemap.draw(g);
            player.draw(g);
        }
        public void draw()
        {
            this.CreateGraphics().DrawImageUnscaled(backBmp,0,0);
            g.FillRectangle(new SolidBrush(Backcolor), 0, 0, WIDTH, HEIGHT);
        }

        public void keyDown(object sender,KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    player.setLeft(true);
                    break;
                case Keys.Right:
                    player.setRight(true);
                    break;
                case Keys.Up:
                    player.setJumping(true);
                    break;
            }
        }
        public void keyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    player.setLeft(false);
                    break;
                case Keys.Right:
                    player.setRight(false);
                    break;
            }
        }
    }
}
