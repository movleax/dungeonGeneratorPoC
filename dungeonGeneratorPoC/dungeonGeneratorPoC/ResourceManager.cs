﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeonGeneratorPoC
{
    class ResourceManager
    {
        private Dictionary<string, PrefabBlueprint> bluePrints;
        private List<ConnectionPoint> NorthPoints;
        private List<ConnectionPoint> SouthPoints;
        private List<ConnectionPoint> EastPoints;
        private List<ConnectionPoint> WestPoints;

        private static ResourceManager instance = null;

        private ResourceManager()
        {
            bluePrints = new Dictionary<string, PrefabBlueprint>();
            NorthPoints = new List<ConnectionPoint>();
            SouthPoints = new List<ConnectionPoint>();
            EastPoints = new List<ConnectionPoint>();
            WestPoints = new List<ConnectionPoint>();
        }

        public static ResourceManager GetInstance()
        {
            if(instance == null)
            {
                instance = new ResourceManager();
            }

            return instance;
        }

        public PrefabBlueprint GetPrefabBluePrintUsingID(string ID)
        {
            return bluePrints[ID];
        }

        public List<ConnectionPoint> GetListOfPairedDirections(Direction dir)
        {
            List<ConnectionPoint> ret = null;

            // set the opposite direction of what our argument is
            switch (dir)
            {
                case Direction.North: ret = SouthPoints; break;
                case Direction.South: ret = NorthPoints; break;
                case Direction.East: ret = WestPoints; break;
                case Direction.West: ret = EastPoints; break;
                default: throw new System.Exception("Invalid direction parameter given");
            }

            return ret;
        }

        public void AddPrefabBluePrint(PrefabBlueprint pfb)
        {
            // Get a list of the blueprints connection points
            List<ConnectionPoint> BluePrintDoorWays = pfb.GetConnectionPoints();

            // go through each of the connection points and add them to appropraite lists
            foreach(var b in BluePrintDoorWays)
            {
                switch(b.GetDirection())
                {
                    case Direction.North: NorthPoints.Add(b); break;
                    case Direction.South: SouthPoints.Add(b); break;
                    case Direction.East: EastPoints.Add(b); break;
                    case Direction.West: WestPoints.Add(b); break;
                }
            }

            // finally add the prefab blueprint
            bluePrints.Add(pfb.GetPrefabID(), pfb);
        }
    }
}
