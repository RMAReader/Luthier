using SharpHelper;
using System.Collections.Generic;
using System.Drawing;

namespace Luthier.Model.GraphicObjects
{
    public interface IDrawableTextured
    {
        void GetVertexAndIndexLists(ref List<TangentVertex> vertices, ref List<int> indices);
        Image Texture { get; }
        string TextureName { get; }
    }
}
