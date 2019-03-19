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
    public partial class DiscDialog : Form
    {
        public double Radius { get; set; }

        public DiscDialog(double radius)
        {
            InitializeComponent();

            Radius = radius;
            radiusTextBox.Text = $"{Radius:F3}";
        }

        private void confirmValueButton_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(radiusTextBox.Text, out double r))
            {
                Radius = r;
            }
            else
            {
                radiusTextBox.Text = $"{Radius:F3}";
            }
        }
    }
}
