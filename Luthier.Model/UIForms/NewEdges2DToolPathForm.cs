using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathSpecification;
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

    public partial class NewEdges2DToolPathForm : Form
    {
        public MouldEdgeSpecification ToolPathSpecification { get; private set; }



        public NewEdges2DToolPathForm(MouldEdgeSpecification toolPathSpecification)
        {
            InitializeComponent();

            ToolPathSpecification = toolPathSpecification;

            UpdateFields();
        }


        private void ConfirmValuesButton_Click(object sender, EventArgs e)
        {
            if(!Double.TryParse(textBoxSafeHeight.Text, out double safeHeight))
            {
                MessageBox.Show("Invalid Safe Height");
                return;
            }
            var cutHeights = new List<double>();
            foreach (var value in textBoxCutHeights.Text.Split(','))
            {
                if (!Double.TryParse(value, out double cutHeight))
                {
                    MessageBox.Show("Invalid Cut Heights");
                    return;
                }
                cutHeights.Add(cutHeight);
            }
            if (!Int32.TryParse(textBoxCuttingHorizontalFeedRate.Text, out int cuttingHorizontalFeedRate))
            {
                MessageBox.Show("Invalid Cutting Horizontal FeedRate");
                return;
            }
            if (!Int32.TryParse(textBoxCuttingVerticalFeedRate.Text, out int cuttingVerticalFeedRate))
            {
                MessageBox.Show("Invalid Cutting Vertical FeedRate");
                return;
            }
            if (!Double.TryParse(textBoxToolDiameter.Text, out double toolDiameter))
            {
                MessageBox.Show("Invalid Tool Diameter");
                return;
            }
            if (!Int32.TryParse(textBoxSpindleSpeed.Text, out int spindleSpeed))
            {
                MessageBox.Show("Invalid Spindle Speed");
                return;
            }
            if (!Int32.TryParse(textBoxNumberOfCopies.Text, out int numberOfCopies))
            {
                MessageBox.Show("Invalid number of copies");
                return;
            }

            ToolPathSpecification.SafeHeight = safeHeight;
            ToolPathSpecification.CutHeights = cutHeights;
            ToolPathSpecification.CuttingHorizontalFeedRate = cuttingHorizontalFeedRate;
            ToolPathSpecification.CuttingHorizontalFeedRate = cuttingVerticalFeedRate;
            ToolPathSpecification.Tool.Diameter = toolDiameter;
            ToolPathSpecification.SpindleSpeed = spindleSpeed;
            ToolPathSpecification.IsInsideCut = checkBoxIsIndideCut.Checked;
            ToolPathSpecification.IsCutFromUnderneath = checkBoxIsUndersideCut.Checked;
            ToolPathSpecification.NumberOfCopies = numberOfCopies;

            ToolPathSpecification.Calculate();
            ToolPathSpecification.Model.HasChanged = true;
        }

        private void buttonResetValues_Click(object sender, EventArgs e)
        {
            UpdateFields();
        }

        private void buttonUpdateSourceCurves_Click(object sender, EventArgs e)
        {
            ToolPathSpecification.BoundaryPolygonKey = new List<UniqueKey>();
            foreach (var obj in ToolPathSpecification.Model.VisibleObjects().Where(x => x is IPolygon2D))
            {
                ToolPathSpecification.BoundaryPolygonKey.Add(obj.Key);
            }
        }

        private void UpdateFields()
        {
            textBoxSafeHeight.Text = $"{ToolPathSpecification.SafeHeight:0.00}";
            textBoxCuttingHorizontalFeedRate.Text = $"{ToolPathSpecification.CuttingHorizontalFeedRate}";
            textBoxCuttingVerticalFeedRate.Text = $"{ToolPathSpecification.CuttingVerticalFeedRate}";
            textBoxToolDiameter.Text = $"{ToolPathSpecification.Tool.Diameter:0.00}";
            textBoxSpindleSpeed.Text = $"{ToolPathSpecification.SpindleSpeed}";

            var cutHeights = new StringBuilder();
            foreach (var h in ToolPathSpecification.CutHeights)
            {
                if(cutHeights.Length == 0)
                {
                    cutHeights.Append($"{h}");
                }
                else
                {
                    cutHeights.Append($",{h}");
                }
            }
            textBoxCutHeights.Text = cutHeights.ToString();
        }


    }
}
