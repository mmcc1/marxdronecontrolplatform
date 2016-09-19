using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;

namespace Marx_Drone_Control_Platform
{
    public partial class Form1 : Form
    {
        private int droneselection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMapProviders.GoogleHybridMap;
            gMapControl1.MaxZoom = 17;
            gMapControl1.MinZoom = 1;
            gMapControl1.Zoom = 4;
        }

        private void aRDrone20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            droneselection = 1;
        }

        private void armToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Boot the bitch
        }

    }
}
