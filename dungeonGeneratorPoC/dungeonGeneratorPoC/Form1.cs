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
        private GameManager gm = null;
        private Timer t = null;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            GameManager.SetContextWindow(this);
            gm = GameManager.GetInstance();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            gm.Draw(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            t = new Timer();
            t.Interval = 125;
            t.Enabled = true;
            t.Start();
            t.Tick += new EventHandler(TimerTickEvent);

        }

        private void TimerTickEvent(object sender, EventArgs e)
        {
            gm.GenerateNewPiece();
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            t.Stop();
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
            t.Start();
        }
    }
}
