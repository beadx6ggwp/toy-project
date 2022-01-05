using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOXX
{
    public partial class Form1 : Form
    {
        Label[,] label_arr = new Label[3, 3];
        RadioButton[] radio = new RadioButton[4];
        bool gameover, mode, turn;
        string aiox,playerox;//給電腦判斷用
        int ox;//雙人順序
        int r;
        int count = 0;
        //true:O, palyer false:X, AI
        public Form1()
        {
            InitializeComponent();
            label_arr = new Label[3, 3] { { label1, label2, label3 }, 
                                          { label4, label5, label6 }, 
                                          { label7, label8, label9 } };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ox = 0;
            r = 1;
            textBox1.Text = "1";
            gameover = false;
            this.BackColor = Color.SkyBlue;
            this.Text = "________OOXX井字遊戲________";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Location= new Point(500, 200);
            button1.BackColor = Color.LightBlue;
            //設定按鈕外觀
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    label_arr[i, j].Text="";
                    label_arr[i, j].BackColor = Color.BlanchedAlmond;
                    label_arr[i, j].Click += new System.EventHandler(label_Click);
                }
            }
            label10.BackColor = Color.LightBlue;
            label11.BackColor = Color.LightBlue;
            label12.BackColor = Color.Yellow;
            turn_AI.Click += new System.EventHandler(radio_Click);
            turn_player.Click += new System.EventHandler(radio_Click);
            mode_AI.Click += new System.EventHandler(radio_Click);
            mode_player.Click += new System.EventHandler(radio_Click);
            groupBox1.BackColor = Color.LightBlue;
            groupBox2.BackColor = Color.LightBlue;
            groupBox3.BackColor = Color.LightBlue;
            mode_AI.Checked = true;
            turn_player.Checked = true;
            label12.Text = "100.00%";
            MessageBox.Show("請先選擇模式與難度","OOXX");
        }
        private void radio_Click(object sender, EventArgs e)
        {
            RadioButton rad = (RadioButton)sender;
            switch (rad.Name)
            {
                case "turn_player":
                    turn = true;
                    aiox = "X";
                    playerox = "O";
                    break;
                case "turn_AI":
                    turn = false;
                    aiox = "O";
                    playerox = "X";
                    break;
                case "mode_player":
                    mode = true;
                    break;
                case "mode_AI":
                    mode = false;
                    break;
            }
            gameover = false;
            clear();
            if (turn == true) ox = 0;
            else ox = 1;            
            if (mode == false && turn == false) { computer(); return; }//電腦先攻
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clear();
            gameover = false;
            if (turn == true) { ox = 0; }
            else { ox = 1; }
            if (mode == false && turn == false) { computer(); }
        }
        private void clear()
        {
            count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    label_arr[i, j].Text = "";
                }
            }
        }
        private void label_Click(object sender, EventArgs e)
        {
            Label lab = (Label)sender;//取得當前作用的Label
            List<Label> placeall = new List<Label>();//所有空格
            
            if (gameover == true)
            {
                return;
            }
            if (lab.Text != "")
            {
                MessageBox.Show("請選其他位置");
                return;
            }
            count++;
            if (ox%2==0)
            {
                lab.Text = "O";
                if (wincheak())
                {
                    MessageBox.Show("Winner is O");
                    gameover = true;
                    return;
                }
                if (count == 9)
                {
                    MessageBox.Show("平局");
                    count = 0;
                    gameover = true;
                }
            }
            else
            {
                lab.Text = "X";
                if (wincheak())
                {
                    MessageBox.Show("Winner is X");
                    count = 0;
                    gameover = true;
                    return;
                }
                if (count == 9)
                {
                    MessageBox.Show("平局");
                    count = 0;
                    gameover = true;
                }
            }
            if (mode == false)
            {
                computer();
                if (wincheak())
                {
                    MessageBox.Show("Winner is AI");
                    count = 0;
                    gameover = true;
                }
                if (count == 9)
                {
                    MessageBox.Show("平局");
                    count = 0;
                    gameover = true;
                }
                return;
            }

            foreach (Label l in label_arr)
            {
                if (l.Text == "")
                {
                    placeall.Add(l);
                }
            }
            if (placeall.Count == 0)
            {
                gameover = true;
                MessageBox.Show("平手");
            }
            ox++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                r = int.Parse(textBox1.Text);
                double f = 100/((double)r);
                label12.Text= f.ToString("00.00") + "%";
            }
            catch { }
        }

        private bool wincheak()
        {
            bool iswon=false;
            string a1 = label_arr[0, 0].Text,m = label_arr[1, 1].Text,b1= label_arr[2, 2].Text,
                   a2 = label_arr[0, 2].Text,b2 = label_arr[2, 0].Text;
            for (int i = 0; i < 3; i++)
            {
                string x = "", y = "";
                for (int j = 0; j < 3; j++)
                {
                    x += label_arr[i, j].Text;
                    y += label_arr[j, i].Text;
                }
                switch(x){
                    case "OOO": return true;
                    case "XXX": return true;
                }
                switch (y)
                {
                    case "OOO": return true;
                    case "XXX": return true;
                }
            }
            if ((a1 == "O" && m == "O" && b1 == "O")|| (a2 == "O" && m == "O" && b2 == "O")){ return true;}            
            if ((a1 == "X" && m == "X" && b1 == "X")|| (a2 == "X" && m == "X" && b2 == "X")){ return true;}

            return iswon;
        }

        
        Random ran = new Random();

        private void computer()
        {
            count++;//平手
            if (AI(aiox)) { return; }
            if (AI(playerox)) { return; }

            //----------------------------------------------------------------------------------
            List<Label> placeall = new List<Label>();//所有空格
            List<Label> connor_ = new List<Label>();
            List<Label> side_ = new List<Label>();
            Label[] connor = new Label[4] { label_arr[0, 0], label_arr[0, 2], label_arr[2, 0], label_arr[2, 2] };
            Label[] side = new Label[4] { label_arr[1, 0], label_arr[1, 2], label_arr[0, 1], label_arr[2, 1] };

            foreach (Label l in label_arr)//如果中間被搶,只下角落
            {
                if (l.Text == "")
                {
                    placeall.Add(l);
                }
            }
            foreach (Label l in connor)
            {
                if (l.Text == "")
                {
                    connor_.Add(l);
                }
            }
            foreach (Label l in side)
            {
                if (l.Text == "")
                {
                    side_.Add(l);
                }
            }
            if (placeall.Count != 0)//防止最後一格出現錯誤
            {
                if (connor[0].Text == "" || connor[1].Text == "" || connor[2].Text == "" || connor[3].Text == ""&&ran.Next(0,r)==0)//如果角落沒棋
                {
                    string x1 = "", y1 = "", x0 = "", y0 = "", x2 = "", y2 = "";
                    for (int k = 0; k < 3; k++)
                    {
                        x1 += label_arr[1, k].Text;
                        y1 += label_arr[k, 1].Text;
                        //
                        x0 += label_arr[0, k].Text;//00 01 02
                        y0 += label_arr[k, 0].Text;//00 10 20
                        x2 += label_arr[2, k].Text;//20 21 22
                        y2 += label_arr[k, 2].Text;//02 12 22
                    }
                    if (label_arr[1, 1].Text == aiox && (side[0].Text==""&& side[1].Text == "" || side[2].Text == "" && side[3].Text == ""))//如果中間有自己的棋 AND 橫兩側OR直兩側 任一為空 就放邊邊
                    {
                        placeall.Clear();
                        if (connor[0].Text == playerox && connor[3].Text == playerox && side_.Count != 0) { side_[ran.Next(0, side_.Count)].Text = aiox; return; }
                        if (connor[1].Text == playerox && connor[2].Text == playerox && side_.Count != 0) { side_[ran.Next(0, side_.Count)].Text = aiox; return; }
                        if (y0 == playerox && x0 == playerox && connor[0].Text == "") { connor[0].Text = aiox; return; }
                        if (y0 == playerox && x2 == playerox && connor[2].Text == "") { connor[2].Text = aiox; return; }
                        if (y2 == playerox && x0 == playerox && connor[1].Text == "") { connor[1].Text = aiox; return; }
                        if (y2 == playerox && x2 == playerox && connor[3].Text == "") { connor[3].Text = aiox; return; }
                        if (x1 == aiox)
                        {
                            foreach (Label l in label_arr)
                            {
                                if (l.Text==""&&(l.Name != label_arr[0, 1].Name && l.Name != label_arr[2, 1].Name))
                                {
                                    placeall.Add(l);
                                }
                            }
                            placeall[ran.Next(0, placeall.Count)].Text = aiox;
                            return;
                        }
                        if (y1 == aiox)
                        {
                            foreach (Label l in label_arr)
                            {
                                if (l.Text==""&&(l.Name != label_arr[1, 0].Name && l.Name != label_arr[1, 2].Name))
                                {
                                    placeall.Add(l);
                                }
                            }
                            placeall[ran.Next(0, placeall.Count)].Text = aiox;
                            return;
                        }
                        
                        side_[ran.Next(0, side_.Count)].Text = aiox;
                        return;
                    }
                    if (y0 == "" && x0 == "" && connor[3].Text == "") { connor[3].Text = aiox; return; }
                    if (y0 == "" && x2 == "" && connor[1].Text == "") { connor[1].Text = aiox; return;}
                    if (y2 == "" && x0 == "" && connor[2].Text == "") { connor[2].Text = aiox; return; }
                    if (y2 == "" && x2 == "" && connor[0].Text == "") { connor[0].Text = aiox; return; }
                    connor_[ran.Next(0,connor_.Count)].Text = aiox;
                }
                else
                {
                    placeall[ran.Next(0, placeall.Count)].Text = aiox;
                }
            }
        }

        private bool AI(string ox_turn)//ATK:aiox   DEF:playerox
        {
            ox_turn += ox_turn;
            List<Label> side_ = new List<Label>();
            Label[] Slash1 = new Label[3] { label_arr[0, 0], label_arr[1, 1], label_arr[2, 2] };
            Label[] Slash2 = new Label[3] { label_arr[0, 2], label_arr[1, 1], label_arr[2, 0] };
            string s1 = "", s2 = "";
            //以迴圈逐一偵測條件
            if (label_arr[1, 1].Text == "" && ran.Next(0, r) == 0)//亂數機率可決定困難度
            {
                label_arr[1, 1].Text = aiox;
                return true;
            }
            for (int i = 0; i < 3; i++)
            {
                string x = "", y = "";
                for (int j = 0; j < 3; j++)
                {
                    x += label_arr[i, j].Text;
                    y += label_arr[j, i].Text;
                }
                if (x == ox_turn && ran.Next(0, r) == 0)//
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (label_arr[i, k].Text == "")
                        {
                            label_arr[i, k].Text = aiox;
                            return true;
                        }
                    }
                }
                if (y == ox_turn && ran.Next(0, r) == 0)//
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (label_arr[k, i].Text == "")
                        {
                            label_arr[k, i].Text = aiox;
                            return true;
                        }
                    }
                }
                s1 += Slash1[i].Text;
                s2 += Slash2[i].Text;
            }
            if (s1 == ox_turn && ran.Next(0, r) == 0)//
            {
                for (int k = 0; k < 3; k++)
                {
                    if (Slash1[k].Text == "")
                    {
                        Slash1[k].Text = aiox;
                        return true;
                    }
                }
            }
            if (s2 == ox_turn && ran.Next(0, r) == 0)//
            {
                for (int k = 0; k < 3; k++)
                {
                    if (Slash2[k].Text == "")
                    {
                        Slash2[k].Text = aiox;
                        return true;
                    }
                }
            }
            
            return false;
        }
        
    }
}
