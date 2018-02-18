using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dungeonGeneratorPoC
{
    class GameRectangle : ICollidable<CollideBox>
    {
        private Int32 width, height;
        private Point position;
        private Color rectColor;
        private CollideBox cBox;
        private Stack<Point> savePositions;

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
            cBox = new CollideBox(position, width, height);
            savePositions = new Stack<Point>();
            PushLocation(); // push the first original position of the GameRectangle
        }

        // Copy Constructor
        public GameRectangle(GameRectangle gr)
        {
            this.position.X = gr.position.X;
            this.position.Y = gr.position.Y;
            this.width = gr.width;
            this.height = gr.height;
            this.rectColor = gr.rectColor;
            cBox = new CollideBox(position, width, height);
            savePositions = new Stack<Point>(gr.savePositions.Reverse());
            //PushLocation(); // push the first original position of the GameRectangle
        }

        public void SetColor(Color c)
        {
            rectColor = c;
        }

        public void PushLocationAndAddNewPosition(Point AddPos)
        {
            PushLocation();
            position.X += AddPos.X;
            position.Y += AddPos.Y;
            cBox.X = position.X;
            cBox.Y = position.Y;
        }

        public void PushLocation()
        {
            savePositions.Push(position);
        }

        public Point PopToPreviousLocation()
        {
            // do not pop first location, but set current position to what's first on the stack
            if (savePositions.Count > 1)
                position = savePositions.Pop();
            else
                position = savePositions.Peek();

            cBox.X = position.X;
            cBox.Y = position.Y;

            return position;
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

            draw(g);

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

        public CollideBox GetCollisionBox()
        {
            return cBox;
        }

        public bool CheckCollision(CollideBox cb)
        {
            // if we are in the x and y bounds
            if(   cBox.X < cb.X + cb.W  
               && cBox.X + cBox.W > cb.X
               && cBox.Y < cb.Y + cb.H
               && cBox.Y + cBox.H > cb.Y)
            {
                return true;
            }

            return false;
        }
    }
}
