using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.UI.ToolPathForms
{
    public partial class Curve : Form
    {
        public Curve()
        {
            InitializeComponent();
            //this.TopLevel = false;
            //this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void Curve_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
