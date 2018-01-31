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

        public Form1()
        {
            InitializeComponent();

            string prefabContentFolder = ConfigurationManager.AppSettings["prefabFolder"];
            string[] filePaths = Directory.GetFiles(prefabContentFolder, "*.txt");

            foreach (var fp in filePaths)
            {
                PrefabPieces.Add(new Prefab(fp));
            }
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
