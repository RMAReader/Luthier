using Luthier.Model.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController
{
    public interface IMouseController
    {
        ViewMapper2D ViewMapper { get; set; }

        void MouseLeftButtonDown(int x, int y);
        void MouseLeftButtonUp(int x, int y);
        void MouseRightButtonDown(int x, int y);
        void MouseRightButtonUp(int x, int y);
        void MouseMiddleButtonDown(int x, int y);
        void MouseMiddleButtonUp(int x, int y);
        void MouseMove(int x, int y);
        void MouseWheel(int x, int y, int delta);
        void Close();

    }
}
