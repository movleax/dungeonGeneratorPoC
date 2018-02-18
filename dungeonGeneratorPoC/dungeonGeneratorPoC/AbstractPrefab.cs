using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeonGeneratorPoC
{
    abstract class AbstractPrefab : ICollidable<List<GameRectangle>>
    {
        protected List<ConnectionPoint> Doorways = new List<ConnectionPoint>();
        protected List<GameRectangle> rectangles = new List<GameRectangle>();
        protected Point position;
        protected Color color;
        protected string prefabID;

        // do not allow abstract prefab objects to be made with the default constructor in the outside world
        protected AbstractPrefab()
        {
            Random rand = RandomManager.GetRandomInstance();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            position = new Point(0, 0);
            prefabID = Guid.NewGuid().ToString();
        }

        public Point GetPosition()
        {
            return position;
        }

        public void SetPosition(Point NewPos)
        {
            Point deltaPosition = new Point(NewPos.X - position.X, NewPos.Y - position.Y);
            position = NewPos;

            // update Connection point Positions
            foreach (var cp in Doorways)
            {
                Point newRectanglePosition = cp.Position;
                newRectanglePosition.X = deltaPosition.X + newRectanglePosition.X;
                newRectanglePosition.Y = deltaPosition.Y + newRectanglePosition.Y;
                cp.Position = newRectanglePosition;
            }

            // update GameRectangles Position
            foreach (var rect in rectangles)
            {
                rect.PopToPreviousLocation();
                rect.PushLocationAndAddNewPosition(position);
            }
        }

        public List<ConnectionPoint> GetConnectionPoints()
        {
            return Doorways;
        }

        public string GetPrefabID()
        {
            return prefabID;
        }

        public List<GameRectangle> GetCollisionBox()
        {
            return rectangles;
        }

        public bool CheckCollision(List<GameRectangle> t)
        {
            foreach (var thisRect in rectangles)
            {
                foreach (var tRect in t)
                {
                    if (thisRect.CheckCollision(tRect.GetCollisionBox()))
                        return true;
                }
            }

            return false;
        }
    }
}
