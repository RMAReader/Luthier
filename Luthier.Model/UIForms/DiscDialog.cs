using Luthier.Geometry;
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
        public Disc Disc { get; set; }


        public DiscDialog(Disc disc)
        {
            InitializeComponent();

            Disc = new Disc
            {
                Radius = disc.Radius,
                Centre = disc.Centre,
                Normal = disc.Normal
            };

            UpdateTextBoxes();
        }

        private void confirmValueButton_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(radiusTextBox.Text, out double r)
                && Double.TryParse(xCoordTextBox.Text, out double x)
                && Double.TryParse(yCoordTextBox.Text, out double y)
                && Double.TryParse(zCoordTextBox.Text, out double z))
            {
                Disc.Radius = r;
                Disc.Centre.X = x;
                Disc.Centre.Y = y;
                Disc.Centre.Z = z;
            }
            else
            {
                UpdateTextBoxes();
            }
        }

        private void UpdateTextBoxes()
        {
            radiusTextBox.Text = $"{Disc.Radius:F3}";
            xCoordTextBox.Text = $"{Disc.Centre.X:F3}";
            yCoordTextBox.Text = $"{Disc.Centre.Y:F3}";
            zCoordTextBox.Text = $"{Disc.Centre.Z:F3}";
        }
    }
}
