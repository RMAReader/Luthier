using Luthier.Model.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.MouseController3D
{
    public interface IMouseController3D
    {
        void Bind(IApplicationDocumentModel model);
        void Bind(Camera wvp);

        void MouseClick(object sender, MouseEventArgs e);
        void MouseDoubleClick(object sender, MouseEventArgs e);
        void MouseDown(object sender, MouseEventArgs e);
        void MouseMove(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseEventArgs e);
        void MouseWheel(object sender, MouseEventArgs e);

        int X { get; }
        int Y { get; }

        void Close();
    }
}
