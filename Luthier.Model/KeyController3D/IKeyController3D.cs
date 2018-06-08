using Luthier.Model.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.KeyController3D
{
    public interface IKeyController3D
    {
        void Bind(IApplicationDocumentModel model);
        void Bind(Camera wvp);

        void Close();

        void KeyUp(object sender, KeyEventArgs e);
        void KeyDown(object sender, KeyEventArgs e);
        void KeyPress(object sender, KeyPressEventArgs e);
    }
}
