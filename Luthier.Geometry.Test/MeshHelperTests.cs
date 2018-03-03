using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Geometry;
using g3;

namespace Luthier.Geometry.Test
{
    [TestClass]
    class MeshHelperTests
    {

        [TestMethod]
        public void Test1_CreateSharpMeshFromG3Mesh()
        {
            var vertices = new Vector3d[]
            {
                new Vector3d( 0,0,0 ),
                new Vector3d( 1,0,0 ),
                new Vector3d( 0,1,0 ),
                new Vector3d( 1,1,0 ),
            };

            var triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            var mesh = DMesh3Builder.Build<Vector3d, int, int>(vertices, triangles);

            var smesh = MeshHelper.CreateFromDMesh3(null, mesh);
        }
    }
}
