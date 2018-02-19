using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dungeonGeneratorPoC
{
    class GameManager
    {
        private List<Prefab> PrefabPieces = new List<Prefab>();
        private ResourceManager resMan = ResourceManager.GetInstance();
        private Queue<ConnectionPoint> points = new Queue<ConnectionPoint>();
        private static GameManager gm = null;
        private static Control window = null;

        public static void SetContextWindow(Control g)
        {
            if (g == null)
            {
                throw new System.ArgumentNullException("Control g", "g cannot be Null");
            }

            window = g;
        }

        public static GameManager GetInstance()
        {
            if(window == null)
            {
                throw new System.NullReferenceException("Must define window context before requesting an instance of GameManager!");
            }

            if(gm == null)
            {
                gm = new GameManager();
            }

            return gm;
        }

        private GameManager()
        {
            if (window == null)
            {
                throw new System.NullReferenceException("Must define window context before creating an instance of GameManager!");
            }

            // Get the prefabrication directory from our app config value, then get a list of .txt files from that path.
            string prefabContentFolder = ConfigurationManager.AppSettings["prefabFolder"];
            string[] filePaths = Directory.GetFiles(prefabContentFolder, "*.txt");

            // Given our filePaths we found above, create a Prefab and add to our PrefabPieces list
            foreach (var fp in filePaths)
            {
                resMan.AddPrefabBluePrint(new PrefabBlueprint(fp));
            }

            // add an arbitrary point to the middle of the screen
            points.Enqueue(new ConnectionPoint(new Point(window.ClientRectangle.Width / 2, window.ClientRectangle.Height / 2), "asdfasdfasdf", Direction.East));

            //add a new prefab piece
            FigureOutConnectionPiece();
        }

        public void GenerateNewPiece()
        {
            if (window == null)
            {
                throw new System.NullReferenceException("Must define window context before Generating a new piece!");
            }

            bool figuredOutNewPiece = false;
            // try up tp 25 times
            for (int i = 0; i < 25 && !figuredOutNewPiece; i++)
                figuredOutNewPiece = FigureOutConnectionPiece();
            window.Update();
            window.Refresh();
        }

        private bool FigureOutConnectionPiece()
        {
            Random rand = RandomManager.GetRandomInstance();
            ConnectionPoint randCp = null;
            PrefabBlueprint pfb = null;
            bool prefabBlueprintPieceFound = false;

            if (points.Count <= 0)
            {
                return false;
            }

            // pick a random point from our list
            ConnectionPoint cp = points.Peek();

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

                // don't check for collisions if we don't have any pieces placed
                if (PrefabPieces.Count > 0)
                {
                    // check collisions. If the piece doesn't collide with anything, then we know found our piece to place.
                    foreach (var pieces in PrefabPieces)
                    {
                        prefabBlueprintPieceFound = !pieces.CheckCollision(pfb.GetCollisionBox());

                        // if we have collided into anything let's break
                        if (prefabBlueprintPieceFound == false)
                            break;
                    }
                }
                else
                {
                    prefabBlueprintPieceFound = true;
                }
            }

            // if we did not find a piece from the last step above, then we need to remove the connection point
            if (prefabBlueprintPieceFound == false || pfb == null)
            {
                points.Dequeue();
                return false;
            }

            // We can now Generate a new PrefabPiece. This will Create a new Prefab piece with the current calues of the PrefabBluePrint
            Prefab p = pfb.GeneratePrefabPiece();

            // Reset the Prefab BluePrint
            pfb.SetPosition(new Point(0, 0));

            // Get rid of the Connection Point that we got from RandCp before adding it to our points list
            p.RemoveConnectionPoint(randCp.ID);

            // Now remove the Connection Point in our points list
            points.Dequeue();

            // Add all of the connection points from our newly generated Prefab to our points list
            List<ConnectionPoint> temp = p.GetConnectionPoints();
            foreach (var t in temp)
                points.Enqueue(t);

            // now that a pfb has been chosen, make a clone of it to a prefab unit.
            PrefabPieces.Add(p);

            return true;
        }

        public void Draw()
        {
            if (window == null)
            {
                throw new System.NullReferenceException("Must define window context before using Draw!");
            }

            foreach (IDrawable p in PrefabPieces)
            {
                p.Draw(window);
            }
        }
    }
}
