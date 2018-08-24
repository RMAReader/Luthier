using Luthier.Model.Presenter;
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
    public partial class LightingOptions : Form
    {
        private LightData _data;

        public LightingOptions(LightData data)
        {
            _data = data;
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _data.Kd = (float)trackBar2.Value / (trackBar2.Maximum - trackBar2.Minimum);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            _data.Kr = (float)trackBar3.Value / (trackBar3.Maximum - trackBar3.Minimum);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            _data.SpecExpon = trackBar4.Value;
        }
    }
}
