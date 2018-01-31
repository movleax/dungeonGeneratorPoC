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
            
            // Get the prefabrication directory from our app config value, then get a list of .txt files from that path.
            string prefabContentFolder = ConfigurationManager.AppSettings["prefabFolder"];
            string[] filePaths = Directory.GetFiles(prefabContentFolder, "*.txt");

            // Given our filePaths we found above, create a Prefab constructors and add to our PrefabPieces list
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
