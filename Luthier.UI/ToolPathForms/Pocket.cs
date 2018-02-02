using Luthier.CncTool;
using Luthier.Model.MouseController;
using Luthier.Model.ToolPathSpecification;
using Luthier.Model.ToolPathSpecificationFactory;
using Luthier.Model.Presenter;
using Luthier.ToolPath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.CncOperation;
using Luthier.Model.GraphicObjects;

namespace Luthier.UI.ToolPathForms
{
    public partial class Pocket : Form
    {
        private readonly PocketToolPathPresenter presenter;


        public Pocket(PocketToolPathPresenter presenter)
        {
            InitializeComponent();

            this.presenter = presenter;
            SetDataBindings();
 
        }

        private void SetDataBindings()
        {
            textBox1.DataBindings.Add("Text", presenter, "Name", false, DataSourceUpdateMode.OnPropertyChanged);

            numericUpDownCutHeight.DataBindings.Add("Value", presenter, "CutHeight", false, DataSourceUpdateMode.OnPropertyChanged);
            numericUpDownSafeHeight.DataBindings.Add("Value", presenter, "SafeHeight", false, DataSourceUpdateMode.OnPropertyChanged);
            numericUpDownSpindleSpeed.DataBindings.Add("Value", presenter, "SpindleSpeed", false, DataSourceUpdateMode.OnPropertyChanged);
            numericUpDownFeedRate.DataBindings.Add("Value", presenter, "FeedRate", false, DataSourceUpdateMode.OnPropertyChanged);
            numericUpDownStepLength.DataBindings.Add("Value", presenter, "StepLength", false, DataSourceUpdateMode.OnPropertyChanged);

            comboBoxTool.Items.AddRange(presenter.AvailableTools.ToArray());
            comboBoxTool.DataBindings.Add("SelectedItem", presenter, "Tool", false, DataSourceUpdateMode.OnPropertyChanged);

            comboBoxSpindleDirection.Items.Add(EnumSpindleState.OnClockwise);
            comboBoxSpindleDirection.Items.Add(EnumSpindleState.OnCounterClockwise);
            //comboBoxSpindleDirection.DataBindings.Add("SelectedItem", presenter, "SpindleDirection", false, DataSourceUpdateMode.OnPropertyChanged);

            listBox1.DataSource = presenter.BoundaryPolygonKey;
            listBox1.ValueMember = "Key";
            listBox1.DisplayMember = "Name";
        
        }


 

        private void Pocket_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.Cancel == false && e.CloseReason == CloseReason.UserClosing)
            //{
            //    DialogResult result = MessageBox.Show("Would you like to save your changes",
            //                                         "Save?",
            //                                         MessageBoxButtons.YesNoCancel,
            //                                         MessageBoxIcon.Stop);
            //    if (result == DialogResult.Yes)
            //    {
            //        presenter.SaveModelSpecification();
            //    }
            //    else if (result == DialogResult.Cancel)
            //    {
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        this.Close();
            //    }
            //}
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            presenter.SaveModelSpecification();
        }
    }
}
