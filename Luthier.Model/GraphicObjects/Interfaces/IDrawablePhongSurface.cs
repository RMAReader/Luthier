using System;
using System.Collections.Generic;
using SharpHelper;

namespace Luthier.Model.GraphicObjects
{
    public interface IDrawablePhongSurface
    {
        void GetVertexAndIndexLists(ref List<TangentVertex> vertices, ref List<int> indices);
        
    }
}
