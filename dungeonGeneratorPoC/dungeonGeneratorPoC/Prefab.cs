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
        private List<GameRectangle> rectangles = new List<GameRectangle>();
        private Prefab() { }
        private Point position;
        private Color color;

        public Prefab(String PrefabFile)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            position = new Point(0, 0);

            GeneratePrefabPiece(PrefabFile);
        }

        public void draw(Control g)
        {
            foreach(var gameRect in rectangles)
                gameRect.draw(g, this.position);
        }

        void GeneratePrefabPiece(string PrefabFile)
        {
            try
            {
                StreamReader reader = new StreamReader(PrefabFile);
                char ch;

                Point currentPos = new Point(0, 0);
                bool addBlock;

                do
                {
                    ch = (char)reader.Read();
                    ch = Char.ToUpper(ch);
                    addBlock = false;

                    Console.Write(ch);
                    if (ch == 'N')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.North));
                        addBlock = true;
                    }
                    else if (ch == 'S')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.South));
                        addBlock = true;
                    }
                    else if (ch == 'E')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.East));
                        addBlock = true;
                    }
                    else if (ch == 'W')
                    {
                        Doorways.Add(new ConnectionPoint(currentPos, Direction.West));
                        addBlock = true;
                    }
                    else if (ch == 'X')
                    {
                        addBlock = true;
                    }

                    // adjust position
                    if (ch == '\r')
                    {
                        currentPos.X = 0;
                        currentPos.Y += Constants.RectangleChunk;
                    }
                    else if(ch != '\n')
                    {
                        currentPos.X += Constants.RectangleChunk;
                    }

                    // add a game rectangle to our prefab list 
                    if(addBlock)
                        rectangles.Add(new GameRectangle(currentPos.X, currentPos.Y, Constants.RectangleChunk, Constants.RectangleChunk, color));

                } while (!reader.EndOfStream);

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
