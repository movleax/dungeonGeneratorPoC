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
        private Point position;
        private Color rectColor;
        private GameRectangle() { }

        public GameRectangle(Int32 x, Int32 y, Int32 width, Int32 height, Color color)
        {
            this.position.X = x;
            this.position.Y = y;
            this.width = width;
            this.height = height;
            this.rectColor = color;
        }

        public void draw(Control g, Point parentPosition)
        {
            Point tempPos = position;
            position.X += parentPosition.X;
            position.Y += parentPosition.Y;

            draw(g);

            position = tempPos;
        }

        public Point GetPosition()
        {
            return position;
        }

        private void draw(Control g)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(rectColor);
            System.Drawing.Graphics formGraphics;
            formGraphics = g.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(position.X, position.Y, width, height));
            myBrush.Dispose();
            formGraphics.Dispose();
        }
    }
}
