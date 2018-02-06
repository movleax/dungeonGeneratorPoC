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
        private Direction facingDirection;
        private string PrefabPieceID;
        
        public string ID
        {
            get { return PrefabPieceID; }
        }
        
        public bool isConnected
        {
            get { return connected; }
            set { connected = value; }
        }
        
        private ConnectionPoint() { }

        public Direction GetDirection()
        {
            return facingDirection;
        }

        public void setPoint(Point Pos, string p, Direction dir)
        {
            if (connected == false)
                throw new System.Exception("Cannot set a value of a ConnectionPoint if it is being used by a PrefabPiece");

            position = Pos;
            facingDirection = dir;
            PrefabPieceID = p;
            isConnected = false;
        }

        public ConnectionPoint(Point Pos, string p, Direction dir)
        {
            position = Pos;
            facingDirection = dir;
            PrefabPieceID = p;
            isConnected = false;
        }
    }
}
