using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public class ModelChangeEventArgs : EventArgs
    {
        public GraphicObjectBase ObjectAdded;
        public GraphicObjectBase ObjectRemoved;
    }
}
