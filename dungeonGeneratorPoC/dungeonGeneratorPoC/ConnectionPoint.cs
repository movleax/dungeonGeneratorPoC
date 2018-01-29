using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeonGeneratorPoC
{
    enum Direction
    {
        North,
        South,
        East,
        West
    }

    class ConnectionPoint
    {
        private Point position;
        private bool connected;
        Direction facingDirection; 

        public bool isConnected
        {
            get { return connected; }
            set { connected = value; }
        }
        
        private ConnectionPoint() { }
        public ConnectionPoint(Point Pos, Direction dir)
        {
            position = Pos;
            facingDirection = dir;
            isConnected = false;
        }
    }
}
