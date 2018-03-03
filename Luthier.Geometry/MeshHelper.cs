using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;
using SharpHelper;

namespace Luthier.Geometry
{
    public class MeshHelper
    {

        public static SharpMesh CreateFromDMesh3(SharpDevice device, DMesh3 mesh)
        {
            var indices = mesh.TrianglesBuffer.ToArray();
            var vertices = mesh.Vertices().ToArray();
            return SharpMesh.Create(device, vertices, indices);
        }

    }
}
