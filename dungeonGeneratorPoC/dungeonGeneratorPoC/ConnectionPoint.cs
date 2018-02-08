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
        private string cpID;

        public string ownerID
        {
            get { return PrefabPieceID; }
        }

        public string ID
        {
            get { return cpID; }
        }

        public bool isConnected
        {
            get { return connected; }
            set { connected = value; }
        }
        
        private ConnectionPoint() { }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

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

        public ConnectionPoint(ConnectionPoint cp)
        {
            position = cp.position;
            facingDirection = cp.facingDirection;
            PrefabPieceID = cp.PrefabPieceID;
            isConnected = cp.isConnected;

            cpID = Guid.NewGuid().ToString();
        }

        public ConnectionPoint(Point Pos, string p, Direction dir)
        {
            position = Pos;
            facingDirection = dir;
            PrefabPieceID = p;
            isConnected = false;

            cpID = Guid.NewGuid().ToString();
        }
    }
}
