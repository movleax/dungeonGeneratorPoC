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
    class Prefab : AbstractPrefab, IDrawable
    {
        // do not allow prefab objects to be made with the default constructor in the outside world
        private Prefab() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrefabFile">The filepath to the prefab file</param>
        public Prefab(Point Pos, string ID, List<ConnectionPoint> ConnPoints, List<GameRectangle> drawRectangles)
        {
            Random rand = RandomManager.GetRandomInstance();
            color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            position = Pos;
            prefabID = ID;

            Doorways = ConnPoints.Select(cp => new ConnectionPoint(cp)).ToList();
            rectangles = drawRectangles.Select(gr => new GameRectangle(gr)).ToList();
            foreach(var rect in rectangles)
            {
                rect.SetColor(color);
            }
        }

        public void RemoveConnectionPoint(String ID)
        {
            for(int i=0; i < Doorways.Count; i++)
            {
                if (Doorways[i].ID == ID)
                {
                    Doorways.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Draw our GameRectangles that we have generated in GeneratePrefabPiece
        /// </summary>
        /// <param name="g">Forms control</param>
        public void Draw(PaintEventArgs e)
        {
            foreach(var gameRect in rectangles)
                gameRect.draw(e, this.position);
        }
    }
}
