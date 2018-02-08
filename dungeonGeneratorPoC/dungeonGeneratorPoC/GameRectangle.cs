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

        // do not allow the outside world to create GameRectangle objects using the default constructor
        private GameRectangle() { }

        // must use this constructor to make GameRectangle objects
        public GameRectangle(Int32 x, Int32 y, Int32 width, Int32 height, Color color)
        {
            this.position.X = x;
            this.position.Y = y;
            this.width = width;
            this.height = height;
            this.rectColor = color;
        }

        public void SetColor(Color c)
        {
            rectColor = c;
        }

        /// <summary>
        /// In order to draw a GameRectangle, we must include a parent position.
        /// The parent position will be applied to the gameRect's position.
        /// This makes it easier to use this GameRect class with the PreFab class since we only have 
        /// to pass in the prefab pieces position to move this object around in conjunction with other pieces.
        /// Will call the private member draw method
        /// </summary>
        /// <param name="g">form's Control</param>
        /// <param name="parentPosition">The parents position</param>
        public void draw(Control g, Point parentPosition)
        {
            Point tempPos = position;
            position.X += parentPosition.X;
            position.Y += parentPosition.Y;

            draw(g);

            position = tempPos;
        }

        // see the overloaded public draw method
        private void draw(Control g)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(rectColor);
            System.Drawing.Graphics formGraphics;
            formGraphics = g.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(position.X, position.Y, width, height));
            myBrush.Dispose();
            formGraphics.Dispose();
        }

        public Point GetPosition()
        {
            return position;
        }
    }
}
