using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;

namespace MODE
{
    public class HsuAI
    {
        int test = 0;

        int count = 0;
        bool[,,] wins = new bool[572, 16, 16];//[高,二維]

        int[] mywin = new int[572];
        int[] uwin = new int[572];
        public HsuAI()//在建構時就生成
        {
            #region 勝利組合
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        wins[count, i, j + k] = true;
                    }
                    count++;
                }
            }
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        wins[count, j + k, i] = true;
                    }
                    count++;
                }
            }
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        wins[count, i + k, j + k] = true;
                    }
                    count++;
                }
            }
            for (int i = 0; i < 11; i++)
            {
                for (int j = 14; j > 3; j--)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        wins[count, i + k, j - k] = true;
                    }
                    count++;
                }
            }
            #endregion
        }

        public void computer()
        {
            int max = 0;
            int def = 0;
            int x = 0, y = 0;
            mywin = new int[572];
            uwin = new int[572];
            cheak();

            int[,] myscore = new int[15, 15];//攻擊分數
            int[,] uscore = new int[15, 15];//防守分數

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board[i, j] == 0)
                    {
                        for (int k = 0; k < count; k++)
                        {
                            if (wins[k, i, j])
                            {
                                switch (uwin[k])
                                {
                                    case 1:
                                        uscore[i, j] += 10;
                                        break;
                                    case 2:
                                        uscore[i, j] += 100;
                                        break;
                                    case 3:
                                        uscore[i, j] += 1000;
                                        break;
                                    case 4:
                                        uscore[i, j] += 10000;
                                        break;
                                }
                                switch (mywin[k])
                                {
                                    case 1:
                                        myscore[i, j] += 15;
                                        break;
                                    case 2:
                                        myscore[i, j] += 150;
                                        break;
                                    case 3:
                                        myscore[i, j] += 1500;
                                        break;
                                    case 4:
                                        myscore[i, j] += 15000;
                                        break;
                                }
                            }
                        }
                        if (uscore[i, j] > max)
                        {
                            max = uscore[i, j];
                            x = i;
                            y = j;
                        }
                        else if (uscore[i, j] == max)
                        {
                            if (myscore[i, j] > myscore[x, y])
                            {
                                x = i;
                                y = j;
                            }
                        }

                        if (myscore[i, j] > max)
                        {
                            max = myscore[i, j];
                            x = i;
                            y = j;
                        }
                        else if (myscore[i, j] == max)
                        {
                            if (uscore[i, j] > uscore[x, y])
                            {
                                x = i;
                                y = j;
                            }
                        }
                    }
                }
            }
            loc = new Point(x, y);
        }

        private void cheak()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board[i, j] == 1)
                    {
                        for (int k = 0; k < count; k++)
                        {
                            if (wins[k, i, j])
                            {
                                mywin[k] = 6;
                                uwin[k]++;
                            }
                        }
                    }

                    if (board[i, j] == 2)
                    {
                        for (int k = 0; k < count; k++)
                        {
                            if (wins[k, i, j])
                            {
                                mywin[k]++;
                                uwin[k] = 6;
                            }
                        }
                    }
                }
            }
        }
        public int[,] board { get; set; }//當前棋盤
        public Point loc{ get; set; }
        

    }
}
