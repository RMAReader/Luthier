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
    public partial class NewImage : Form
    {
        public NewImage()
        {
            InitializeComponent();
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
                imagePictureBox.Image = System.Drawing.Image.FromFile(imageFilePathTextBox.Text);
            }
        }
    }
}
