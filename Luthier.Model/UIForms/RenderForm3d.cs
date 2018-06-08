using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.UIForms
{
    public partial class RenderForm3d : RenderForm
    {
        public RenderForm3d()
        {
            InitializeComponent();
        }

        public EventHandler DoCurveToolStripItem_Click;
        public EventHandler DoPlaneToolStripMenuItem_Click;
        public EventHandler DoPerspectiveToolStripMenuItem_Click;
        public EventHandler DoOrthonormalToolStripMenuItem_Click;

        private void curveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCurveToolStripItem_Click(sender, e);
        }

        private void planeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPlaneToolStripMenuItem_Click(sender, e);
        }

        private void perspectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPerspectiveToolStripMenuItem_Click(sender, e);
        }

        private void orthonormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoOrthonormalToolStripMenuItem_Click(sender, e);
        }
    }
}
