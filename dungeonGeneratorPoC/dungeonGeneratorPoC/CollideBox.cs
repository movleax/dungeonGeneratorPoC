using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeonGeneratorPoC
{
    class CollideBox
    {
        private Point pos;
        private int width;
        private int height;

        private CollideBox() { }

        public CollideBox(Point p, int Width, int Height)
        {
            this.pos = p;
            this.width = Width;
            this.height = Height;
        }

        public int X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        public int Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        public int W
        {
            get { return width; }
        }

        public int H
        {
            get { return height; }
        }

        public CollideBox GetCollisionBox()
        {
            return this;
        }
    }
}
