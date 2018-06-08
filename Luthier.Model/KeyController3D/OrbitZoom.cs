using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Model.Presenter;
using SharpDX;

namespace Luthier.Model.KeyController3D
{
    public class OrbitZoom : IKeyController3D
    {
        private IApplicationDocumentModel model;
        private Camera _camera;

        public void Bind(IApplicationDocumentModel model)
        {
            this.model = model;
        }

        public void Bind(Camera camera)
        {
            _camera = camera;
        }

        public void Close()
        {
            
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:

                    _camera.Phi = _camera.Phi - 0.1f * (e.Shift ? 3 : 1);
                    _camera.Phi = Math.Min(_camera.Phi, (float)(Math.PI * 0.9));
                    _camera.Phi = Math.Max(_camera.Phi, (float)(Math.PI * 0.1));
                    break;

                case Keys.Down:
                    _camera.Phi = _camera.Phi + 0.1f * (e.Shift ? 3 : 1);
                    _camera.Phi = Math.Min(_camera.Phi, (float)(Math.PI * 0.9));
                    _camera.Phi = Math.Max(_camera.Phi, (float)(Math.PI * 0.1));
                    break;

                case Keys.Left:
                    _camera.Theta = _camera.Theta - 0.1f * (e.Shift ? 3 : 1);
                    break;

                case Keys.Right:
                    _camera.Theta = _camera.Theta + 0.1f * (e.Shift ? 3 : 1);
                    break;

                case Keys.Add:
                    _camera.ZoomFactor /= 1.1f;
                    break;

                case Keys.Subtract:
                    _camera.ZoomFactor *= 1.1f;
                    break;

                case Keys.Home:
                    _camera.Theta = (float)-Math.PI / 2;
                    _camera.Phi = 0.01f;
                    break;
            }
        }

        public void KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            
        }
    }
}
