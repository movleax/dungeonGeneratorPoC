using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dungeonGeneratorPoC
{
    public partial class Form1 : Form
    {
        List<Prefab> PrefabPieces = new List<Prefab>();

        public Form1()
        {
            InitializeComponent();
            Prefab p = new Prefab(@"c:\users\austin\documents\visual studio 2015\Projects\simple_rect\simple_rect\resources\BigRoom.txt");
            PrefabPieces.Add(p);
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
