using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using g3;
using System.Collections.Generic;
using System.Linq;

namespace geometry3Sharp.Tests
{

  

    [TestClass]
    public class Dmesh3Tests
    {
        [TestMethod]
        public void TestMethod1_CreateMesh()
        {
            var vertices = new Vector3d[]
            {
                new Vector3d( 0,0,0 ),
                new Vector3d( 1,0,0 ),
                new Vector3d(0,1,0 ),
                new Vector3d( 1,1,0 ),
            };

            var triangles = new int[]{0,1,2,1,2,3 };
            var mesh = DMesh3Builder.Build<Vector3d,int,int>(vertices, triangles);

            int i = mesh.AppendVertex(new Vector3d(0.5, 1.5, 0));
            mesh.AppendTriangle(2, 3, i);
        }


        [TestMethod]
        public void TestMethod2_ReduceMesh()
        {
            //create uniform squre n * n
            int n = 10;
            var vertices = new List<Vector3d>();
            for (int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    //vertices.Add(new Vector3d(Math.Pow(i*i,0.4), Math.Pow(j*j,0.4), 0));
                    vertices.Add(new Vector3d(i, j, 0));
                }
            }
            var triangles = new List<int>();
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    int sw = i * n + j;
                    int se = sw + 1;
                    int ne = se + n;
                    int nw = sw + n;

                    triangles.AddRange(new int[] { sw, se, ne });
                    triangles.AddRange(new int[] { ne, nw, sw});
                }
            }
            var mesh = DMesh3Builder.Build<Vector3d, int, int>(vertices, triangles);

            Reducer r = new Reducer(mesh);
            //r.SetExternalConstraints(new MeshConstraints());
            //MeshConstraintUtil.FixAllBoundaryEdges(r.Constraints, mesh);
            r.ReduceToTriangleCount(50);

            mesh = new DMesh3(mesh);

            var newtriangles = mesh.TrianglesBuffer;
            var newVertices = mesh.VerticesBuffer;
            var newNormals = mesh.NormalsBuffer;

            var areaList = new List<double>();
            for (int i = 0; i < mesh.MaxTriangleID; i++) areaList.Add(mesh.GetTriArea(i));

            var area = areaList.Sum();
        }

    }
}
