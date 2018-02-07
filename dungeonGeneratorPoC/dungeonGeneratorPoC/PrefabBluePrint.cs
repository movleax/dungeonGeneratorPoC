using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dungeonGeneratorPoC
{
    class PrefabBluePrint
    {
        private List<ConnectionPoint> Doorways = new List<ConnectionPoint>();
        private List<GameRectangle> rectangles = new List<GameRectangle>();
        private Point position;
        private Color color;
        private string prefabID;

        // do not allow prefab objects to be made with the default constructor in the outside world
        private PrefabBluePrint() { }

        /// <summary>
        /// generates a prefab piece given the Prefab pieces filepath
        /// </summary>
        /// <param name="PrefabFile">The filepath to the prefab file</param>
        public PrefabBluePrint(String PrefabFile)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            position = new Point(0, 0);
            prefabID = Guid.NewGuid().ToString();

            GeneratePrefabPiece(PrefabFile);
        }

        public Point GetPosition()
        {
            return position;
        }

        public void SetPosition(Point NewPos)
        {
            position = NewPos;
        }

        public Prefab GeneratePrefabPiece()
        {
            return new Prefab(position, prefabID, Doorways, rectangles);
        }

        public List<ConnectionPoint> GetConnectionPoints()
        {
            return Doorways;
        }

        public string GetPrefabID()
        {
            return prefabID;
        }

        /// <summary>
        /// Going to read char-by-char from the given filepath.
        /// We will generate a gameRectangle at a calculated position if we read an 'X', 'N', 'S', 'E', or 'W'.
        /// Creates a Connection point if we read an 'N', 'S', 'E', or 'W'.
        /// 
        /// 'N', 'S', 'E', and 'W' stands for the direction the prefab piece has a "doorway"( an ability to connect to other prefab pieces ).
        /// 
        /// These "doorways" must point away from the prefab piece.
        /// 
        /// 'N' stands for a doorway pointing North
        /// 'S' stands for a doorway pointing South
        /// 'E' stands for a doorway pointing East
        /// 'W' stands for a doorway pointing West
        /// 
        /// Input is not Case sensitive; values read in will be upp cased.
        /// 
        /// </summary>
        /// <param name="PrefabFile">filepath to the PreFab piece file</param>
        void GeneratePrefabPiece(string PrefabFile)
        {
            try
            {
                // open a stream reader to the prefab file
                StreamReader reader = new StreamReader(PrefabFile);
                char ch;

                // We will use this Point to calculate a GameRectangle's position
                Point currentPos = new Point(0, 0);

                // we will use this boolean to signify if we are creating a GameRectangle or not during each do/while loop iteration
                bool addBlock;

                do
                {
                    // first read our input char and set it to Upper Case
                    ch = (char)reader.Read();
                    ch = Char.ToUpper(ch);

                    // assume that we aren't adding a GameRectangle this current iteration
                    addBlock = false;

                    // check the ascii value and respond appropriately (see function header for details)
                    if (ch == 'N')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, prefabID, Direction.North));
                        addBlock = true;
                    }
                    else if (ch == 'S')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, prefabID, Direction.South));
                        addBlock = true;
                    }
                    else if (ch == 'E')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, prefabID, Direction.East));
                        addBlock = true;
                    }
                    else if (ch == 'W')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, prefabID, Direction.West));
                        addBlock = true;
                    }
                    else if (ch == 'X')
                    {
                        addBlock = true;
                    }

                    // adjust our calculated point position
                    if (ch == '\r')
                    {
                        currentPos.X = 0;
                        currentPos.Y += Constants.RectangleChunk;
                    }
                    else if (ch != '\n')
                    {
                        currentPos.X += Constants.RectangleChunk;
                    }

                    // add a game rectangle to our prefab list 
                    if (addBlock)
                        rectangles.Add(new GameRectangle(currentPos.X, currentPos.Y, Constants.RectangleChunk, Constants.RectangleChunk, color));

                } while (!reader.EndOfStream);

                // clean up
                reader.Close();
                reader.Dispose();
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
