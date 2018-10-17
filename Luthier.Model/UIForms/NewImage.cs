using Luthier.Model.MouseController3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.UIForms
{
    public partial class NewImageForm : Form
    {
        public InsertImage Controller { get; private set; }

        public NewImageForm(InsertImage controller)
        {
            InitializeComponent();

            Controller = controller;
        }

        private void selectImageBrowseButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (File.Exists(imageFilePathTextBox.Text))
            {
                openFileDialog.FileName = imageFilePathTextBox.Text;
            }
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageFilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void imageFilePathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(imageFilePathTextBox.Text))
            {
                Controller.Path = imageFilePathTextBox.Text;

                imagePictureBox.Image = System.Drawing.Image.FromFile(imageFilePathTextBox.Text);
            }
        }

        private void placeImageButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}
