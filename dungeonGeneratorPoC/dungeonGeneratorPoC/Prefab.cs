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
    class Prefab
    {
        private List<ConnectionPoint> Doorways = new List<ConnectionPoint>();
        private GameRectangle rect;
        private Prefab() { }

        public Prefab(String PrefabFile)
        {
            GeneratePrefabPiece(PrefabFile);
        }

        public void draw(Control g)
        {
            rect.draw(g);
        }

        void GeneratePrefabPiece(string PrefabFile)
        {
            try
            {
                StreamReader reader = new StreamReader(PrefabFile);
                char ch;
                Int32 width = new Int32();
                Int32 height = new Int32();

                Point currentPos = new Point(0, 0);

                do
                {
                    ch = (char)reader.Read();
                    Console.Write(ch);
                    if (ch == 'N')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.North));
                    }
                    else if (ch == 'S')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.South));
                    }
                    else if (ch == 'E')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.East));
                    }
                    else if (ch == 'W')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.West));
                    }

                    if (ch == '\r')
                    {
                        currentPos.X = 0;
                        currentPos.Y += Constants.RectangleChunk;

                        if (currentPos.Y > height)
                            height += Constants.RectangleChunk;
                    }
                    else if(ch != '\n')
                    {
                        currentPos.X += Constants.RectangleChunk;

                        if (currentPos.X > width)
                            width += Constants.RectangleChunk;
                    }
                    

                } while (!reader.EndOfStream);

                reader.Close();
                reader.Dispose();

                // lastly create the gameRectangle
                rect = new GameRectangle(200, 200, width, height);

            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
