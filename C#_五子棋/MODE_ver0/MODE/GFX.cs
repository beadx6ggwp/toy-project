using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
namespace MODE
{
    public class GFX
    {
        //EX.叫用方式 GFX 名稱 = New GFX(Graphics 名稱)
        
        public static Graphics gObject;
        public static int gap = 0;//間距
        int r;
        //建構式:若無設立的話,則預設無引數建構式
        public GFX(Graphics g,int Rows,int Size,int slant)//畫布物件,行,最大,偏移
        {
            gObject = g;
            r = slant;
            drawBoard(Rows,Size-2,r);//預設值:15,560
        }

        public void drawBoard(int row,int max,int r)//row:行,max:終點位置,r:偏移位置
        {
            row--;
            gap = max / row;//間隔
            //標示點
            SolidBrush B = new SolidBrush(Color.Black);
            Rectangle[] rect = new Rectangle[] { mid_p(3,3,8,gap) ,mid_p(3,11,8,gap),mid_p(7,7,8,gap) ,mid_p(11,3,8,gap),mid_p(11,11,8,gap)};
            gObject.FillRectangles(B, rect);

            Pen P_line = new Pen(Color.Black, 2);
            Rectangle[] re = new Rectangle[5];
            List<Rectangle> lr = new List<Rectangle>();
            for (int i = 0; i < row; i++)
            {
                int sp = gap * i;//間距大小
                gObject.DrawLine(P_line, new Point(sp, 0), new Point(sp, max));
                gObject.DrawLine(P_line, new Point(0, sp), new Point(max, sp));
                
            }
            gObject.DrawLine(P_line, new Point(max, 0), new Point(max, max));
            gObject.DrawLine(P_line, new Point(0, max), new Point(max, max));
        }

        private Rectangle mid_p(int x,int y,int s_max,int gap_)//[直角]坐標,範圍,間隔
        {
            int s_min = s_max / 2;
            Rectangle r = new Rectangle(gap_ * x - s_min, gap_ * y - s_min, s_max, s_max);
            return r;
        }
    }
}
