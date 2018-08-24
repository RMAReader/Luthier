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
        private ColorDialog _colorDialog;

        public LightingOptions(LightData data)
        {
            _data = data;
            
            _colorDialog = new ColorDialog();

            InitializeComponent();

            UpdateComponents();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _data.AmbientCoefficient = (float)ambientCoeffTrackBar.Value / (ambientCoeffTrackBar.Maximum - ambientCoeffTrackBar.Minimum);
            UpdateComponents();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _data.DiffuseCoefficient = (float)diffuseCoeffTrackBar.Value / (diffuseCoeffTrackBar.Maximum - diffuseCoeffTrackBar.Minimum);
            UpdateComponents();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            _data.SpecularCoefficient = (float)specularCoeffTrackBar.Value / (specularCoeffTrackBar.Maximum - specularCoeffTrackBar.Minimum);
            UpdateComponents();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            _data.ShininessCoefficient = shininessTrackBar.Value;
            UpdateComponents();
        }


        private void AmbientColorButton_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                _data.AmbientColor = ConvertColor(_colorDialog.Color);
                UpdateComponents();
            }
        }

        private void DiffuseColorButton_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                _data.DiffuseColor = ConvertColor(_colorDialog.Color);
                UpdateComponents();
            }
        }

        private void SpecularColorButton_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                _data.SpecularColor = ConvertColor(_colorDialog.Color);
                UpdateComponents();
            }
        }




        private void UpdateComponents()
        {

            ambientCoeffTrackBar.Value = (int)((1 - _data.AmbientCoefficient) * ambientCoeffTrackBar.Minimum + _data.AmbientCoefficient * ambientCoeffTrackBar.Maximum);
            diffuseCoeffTrackBar.Value = (int)((1 - _data.DiffuseCoefficient) * diffuseCoeffTrackBar.Minimum + _data.DiffuseCoefficient * diffuseCoeffTrackBar.Maximum);
            specularCoeffTrackBar.Value = (int)((1 - _data.SpecularCoefficient) * specularCoeffTrackBar.Minimum + _data.SpecularCoefficient * specularCoeffTrackBar.Maximum);
            shininessTrackBar.Value = (int) Math.Min(shininessTrackBar.Maximum, Math.Max(shininessTrackBar.Minimum,_data.ShininessCoefficient));

            AmbientColorButton.BackColor = ConvertColor(_data.AmbientColor * _data.AmbientCoefficient);
            DiffuseColorButton.BackColor = ConvertColor(_data.DiffuseColor * _data.DiffuseCoefficient);
            SpecularColorButton.BackColor = ConvertColor(_data.SpecularColor * _data.SpecularCoefficient);
        }


        private SharpDX.Vector3 ConvertColor(Color input)
        {
            return new SharpDX.Vector3(
                    (float)input.R / 255,
                    (float)input.G / 255,
                    (float)input.B / 255);
        }

        private Color ConvertColor(SharpDX.Vector3 input)
        {
            return Color.FromArgb(
                    (int)(input.X * 255),
                    (int)(input.Y * 255),
                    (int)(input.Z * 255));
        }

      

 
    }
}
