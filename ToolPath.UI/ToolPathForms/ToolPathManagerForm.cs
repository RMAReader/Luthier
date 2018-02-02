using Luthier.CncOperation;
using Luthier.Model;
using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathSpecification;
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

namespace Luthier.UI.ToolPathForms
{
    public partial class ToolPathManagerForm : Form
    {

        private readonly ToolPathManagerPresenter presenter;

        public ToolPathManagerForm(ToolPathManagerPresenter presenter)
        {
            this.presenter = presenter;

            InitializeComponent();
            SetDataBindings();
        }


        private void SetDataBindings()
        {
            listBox1.DataSource = presenter.ToolPaths;
            listBox1.ValueMember = "Key";
            listBox1.DisplayMember = "Name";
        }


        private void pocketToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var pocketPresenter = presenter.NewPocketToolPath();
            var drawingPresenter = presenter.NewDrawing2dPresenter();
            drawingPresenter.MouseController = pocketPresenter.MouseController;

            var pocketForm = new Pocket(pocketPresenter);
            var drawingForm = new DrawingForm2D(drawingPresenter);//new Drawing2DMouseController(pocketPresenter.MouseController, pocketPresenter);
            drawingForm.ToolPathMode();

            pocketForm.Visible = true;
            pocketForm.Show();
            
            drawingForm.Visible = true;
            drawingForm.Show();
        }

        private void curveToolStripMenuItem_Click(object sender, EventArgs e)
        {
             
        }

        private void editSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(ToolPathSpecificationBase item in listBox1.SelectedItems)
            {
                if(item is PocketSpecification)
                {
                    var pocketPresenter = presenter.EditPocketToolPath(item as PocketSpecification);
                    var drawingPresenter = presenter.NewDrawing2dPresenter();
                    drawingPresenter.MouseController = pocketPresenter.MouseController;

                    var pocketForm = new Pocket(pocketPresenter);
                    var drawingForm = new DrawingForm2D(drawingPresenter);//new Drawing2DMouseController(pocketPresenter.MouseController, pocketPresenter);
                    drawingForm.ToolPathMode();

                    //specificationForms.Add(spec.Key, pocketForm);
                    pocketForm.Visible = true;
                    pocketForm.Show();
                }
                else if(item is CurveSpecification)
                {
                 
                }
            }
        }

        private void ToolPathManagerForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void calculateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolPathSpecificationBase spec in listBox1.Items)
            {
                if (spec != null)
                {
                    presenter.CalculateToolPaths(spec);
                }
            }
        }

        private void calculateSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolPathSpecificationBase spec in listBox1.SelectedItems)
            {
                if (spec != null)
                {
                    presenter.CalculateToolPaths(spec);
                }
            }
        }
    }
}
