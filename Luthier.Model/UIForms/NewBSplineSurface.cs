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
    public partial class NewBSplineSurface : Form
    {
        public NewBSplineSurface()
        {
            InitializeComponent();
        }

        public int NumberOfControlPointsU => (int)numericUpDownUControlPoints.Value;
        public int NumberOfControlPointsV => (int)numericUpDownVControlPoints.Value;
    }
}
