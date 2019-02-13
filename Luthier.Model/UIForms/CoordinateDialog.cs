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
    public partial class CoordinateDialog : Form
    {
        private double[] _currentCoordinates;

        public CoordinateDialog(double[] currentCoordinates)
        {
            InitializeComponent();

            _currentCoordinates = currentCoordinates;

            textBoxXCoord.Text = $"{_currentCoordinates[0]:F3}";
            textBoxYCoord.Text = $"{_currentCoordinates[1]:F3}";
            textBoxZCoord.Text = $"{_currentCoordinates[2]:F3}";

        }

        public double[] Coordinates => _currentCoordinates;

        private void textBoxXCoord_TextChanged(object sender, EventArgs e) {}

        private void textBoxYCoord_TextChanged(object sender, EventArgs e) {}

        private void textBoxZCoord_TextChanged(object sender, EventArgs e) {}

        private void buttonConfirmValues_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(textBoxXCoord.Text, out double x)
                && Double.TryParse(textBoxYCoord.Text, out double y) 
                && Double.TryParse(textBoxZCoord.Text, out double z))
            {
                _currentCoordinates[0] = x;
                _currentCoordinates[1] = y;
                _currentCoordinates[2] = z;
            }
            else
            {
                textBoxXCoord.Text = $"{_currentCoordinates[0]:F3}";
                textBoxYCoord.Text = $"{_currentCoordinates[1]:F3}";
                textBoxZCoord.Text = $"{_currentCoordinates[2]:F3}";
            }
        }
    }
}
