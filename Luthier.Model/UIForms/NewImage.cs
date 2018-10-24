using Luthier.Model.MouseController3D;
using Luthier.Model.Presenter;
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
        private readonly ViewPort3DPresenter _presenter;

        private readonly InsertImage _insertImageController;

        public NewImageForm(ViewPort3DPresenter presenter, InsertImage insertImageController)
        {
            InitializeComponent();

            _insertImageController = insertImageController;
            _presenter = presenter;
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
                _insertImageController.Path = imageFilePathTextBox.Text;

                imagePictureBox.Image = System.Drawing.Image.FromFile(imageFilePathTextBox.Text);

                placeImageButton.Enabled = true;
            }
            else
            {
                placeImageButton.Enabled = false;
            }
        }

        private void placeImageButton_Click(object sender, EventArgs e)
        {
            if(_insertImageController.Canvas != null)
            {
                _presenter.SetMouseController(_insertImageController);
            }
            else
            {
                MessageBox.Show("A plane must be selected in order to insert an image.");
            }
            
        }

       
    }
}
