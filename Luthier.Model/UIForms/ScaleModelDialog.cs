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
    public partial class ScaleModelDialog : Form
    {
        public ScaleModelDialog()
        {
            InitializeComponent();
        }

        public double GetResult()
        {
            if (Double.TryParse(textBox1.Text, out double result))
                return result;

            return 1;
        }
    }
}
