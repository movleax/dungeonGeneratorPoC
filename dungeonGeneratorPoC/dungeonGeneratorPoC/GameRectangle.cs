using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dungeonGeneratorPoC
{
    class GameRectangle
    {
        private Int32 width, height;
        public Point position;
        private Color rectColor;
        private GameRectangle() { }

        public GameRectangle(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            position.X = x;
            position.Y = y;
            this.width = width;
            this.height = height;
            this.rectColor = new Color();

            Random r = new Random();
            this.rectColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
        }

        public void draw(Control g)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics;
            formGraphics = g.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(position.X, position.Y, width, height));
            myBrush.Dispose();
            formGraphics.Dispose();
        }
    }
}
