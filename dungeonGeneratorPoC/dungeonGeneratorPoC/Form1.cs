using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace dungeonGeneratorPoC
{
    public partial class Form1 : Form
    {
        List<Prefab> PrefabPieces = new List<Prefab>();
        ResourceManager resMan = ResourceManager.GetInstance();
        List<ConnectionPoint> points = new List<ConnectionPoint>();

        public Form1()
        {
            InitializeComponent();
            
            // Get the prefabrication directory from our app config value, then get a list of .txt files from that path.
            string prefabContentFolder = ConfigurationManager.AppSettings["prefabFolder"];
            string[] filePaths = Directory.GetFiles(prefabContentFolder, "*.txt");
            
            points.Add(new ConnectionPoint(new Point(ClientRectangle.Width / 2, ClientRectangle.Height / 2), "asdfasdfasdf", Direction.East));

            // Given our filePaths we found above, create a Prefab and add to our PrefabPieces list
            foreach (var fp in filePaths)
            {
                resMan.AddPrefabBluePrint(new PrefabBluePrint(fp));
            }

            // check adding a new prefab piece
            bool retVal_FigureOutConnectionPiece = true;

            for (int i = 0; i < 100; i++)
            {
                retVal_FigureOutConnectionPiece = FigureOutConnectionPiece();
            }
        }

        private bool FigureOutConnectionPiece()
        {
            Random rand = RandomManager.GetRandomInstance();
            ConnectionPoint randCp = null;
            PrefabBluePrint pfb = null;
            bool prefabBlueprintPieceFound = false;

            if (points.Count <= 0)
            {
                return false;
            }

            // pick a random point from our list
            ConnectionPoint cp = points[rand.Next(0, points.Count - 1)];
            
            // get a list of connections that pair with our chosen connectionPoint
            List<ConnectionPoint> cpList = resMan.GetListOfPairedDirections(cp.GetDirection());

            if (cpList.Count <= 0)
            {
                return false;
            }

            // try to find a prefab blueprint piece that will fit. Only do this a couple of times or until find a fitting piece
            for (int i = 0; i < 50 && !prefabBlueprintPieceFound; i++)
            {
                randCp = cpList[rand.Next(0, cpList.Count - 1)];

                // make sure the "random" connection point does not belong to the same prefab blueprint piece. Try to get a unique point, up to 5 times total
                for (int k = 0; k < 5 && randCp.ownerID == cp.ownerID; k++)
                {
                    // get a "random" connection point from the list we get back from the resource manager
                    randCp = cpList[rand.Next(0, cpList.Count - 1)];
                }

                // Get the prefabBluePrint piece given the random connection points ID
                pfb = resMan.GetPrefabBluePrintUsingID(randCp.ownerID);

                // Move the prefabBlueprint piece to the correct location
                Point posCalc = pfb.GetPosition();

                // Get the scalar vector difference between the randCp and the pfb. This is assuming prefabBlueprint pieces X,Y is always at the top left corner
                posCalc.X -= randCp.Position.X;
                posCalc.Y -= randCp.Position.Y;

                // now add this scalar difference to the position of our original connection point
                posCalc.X += cp.Position.X;
                posCalc.Y += cp.Position.Y;

                // finally update the blueprint piece with the new calculated position
                pfb.SetPosition(posCalc);

                // check collisions. If the piece doesn't collide with anything, then we know found our piece to place.
                foreach (var pieces in PrefabPieces)
                {
                    prefabBlueprintPieceFound = !pieces.CheckCollision(pfb.GetCollisionBox());
                }
            }

            // if we did not find a piece from the last step above, then we need to remove the connection point
            if(prefabBlueprintPieceFound == false || pfb == null)
            {
                // Now remove the Connection Point in our points list
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].ID == cp.ID)
                    {
                        points.RemoveAt(i);
                        break;
                    }
                }
            }

            // We can now Generate a new PrefabPiece. This will Create a new Prefab piece with the current calues of the PrefabBluePrint
            Prefab p = pfb.GeneratePrefabPiece();

            // Reset the Prefab BluePrint
            pfb.SetPosition(new Point(0, 0));

            // Get rid of the Connection Point that we got from RandCp before adding it to our points list
            p.RemoveConnectionPoint(randCp.ID);

            // Now remove the Connection Point in our points list
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].ID == cp.ID)
                {
                    points.RemoveAt(i);
                    break;
                }

                return false;
            }

            // Add all of the connection points from our newly generated Prefab to our points list
            points.AddRange(p.GetConnectionPoints());

            // now that a pfb has been chosen, make a clone of it to a prefab unit.
            PrefabPieces.Add(p);

            return true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            foreach(Prefab p in PrefabPieces)
            {
                p.draw(this);
            }

        }
    }
}
