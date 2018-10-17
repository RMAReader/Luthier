using g3;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.Scene3D
{
    class SceneUpdater
    {




        //public void CreateMesh_NurbsSurface(List<Vector3d> vertices, List<Vector3d> normals, List<int> indices)
        //{
        //    vertices.Clear();
        //    normals.Clear();
        //    indices.Clear();

        //    int nU = 20;
        //    int nV = 20;

        //    int indexOffset;
        //    foreach (GraphicNurbSurface surface in model.Where(x => x is GraphicNurbSurface))
        //    {

        //        indexOffset = vertices.Count;

        //        // var points2d = new List<Vector3d>();
        //        for (int i = 0; i < nU; i++)
        //        {
        //            double u = (1 - (double)i / (nU)) * surface.Domain0().Min + (double)i / (nU) * surface.Domain0().Max;
        //            for (int j = 0; j < nV; j++)
        //            {
        //                double v = (1 - (double)j / (nV)) * surface.Domain1().Min + (double)j / (nV) * surface.Domain1().Max;
        //                vertices.Add(new Vector3d(surface.Evaluate(u, v)));
        //                normals.Add(new Vector3d(surface.EvaluateNormal(u, v)));
        //            }
        //        }

        //        for (int i = 0; i < nU - 1; i++)
        //        {
        //            for (int j = 0; j < nV - 1; j++)
        //            {
        //                int sw = i * nV + j + indexOffset;
        //                int se = sw + 1;
        //                int nw = sw + nV;
        //                int ne = nw + 1;
        //                indices.AddRange(new int[] { sw, se, ne, ne, nw, sw });
        //                indices.AddRange(new int[] { sw, ne, se, ne, sw, nw });
        //            }
        //        }
        //    }
        //}

        //public void CreateMesh_NurbsControl(List<Vector3d> vertices, List<Vector3d> normals, List<int> indices)
        //{
        //    vertices.Clear();
        //    normals.Clear();
        //    indices.Clear();
        //    int indexOffset;

        //    foreach (GraphicNurbSurface surface in model.Where(x => x is GraphicNurbSurface))
        //    {

        //        indexOffset = vertices.Count;

        //        for (int i = 0; i < surface.CvCount0; i++)
        //        {
        //            for (int j = 0; j < surface.CvCount1; j++)
        //            {
        //                vertices.Add(new Vector3d(surface.GetCV(i, j)));
        //                normals.Add(new Vector3d(0, 0, 1));
        //            }
        //        }

        //        //vertical
        //        for (int i = 0; i < surface.CvCount0 - 1; i++)
        //        {
        //            for (int j = 0; j < surface.CvCount1; j++)
        //            {
        //                var from = i * surface.CvCount1 + j + indexOffset;
        //                var to = from + surface.CvCount1;
        //                indices.AddRange(new int[] { from, to });
        //            }
        //        }

        //        //horizontal
        //        for (int i = 0; i < surface.CvCount0; i++)
        //        {
        //            for (int j = 0; j < surface.CvCount1 - 1; j++)
        //            {
        //                var from = i * surface.CvCount1 + j + indexOffset;
        //                var to = from + 1;
        //                indices.AddRange(new int[] { from, to });
        //            }
        //        }
        //    }
        //}



        //public void CreateMesh_NurbsCurve(List<Vector3d> vertices, List<Vector3d> normals, List<int> indices)
        //{
        //    vertices.Clear();
        //    normals.Clear();
        //    indices.Clear();

        //    foreach (NurbsCurve curve in model.Where(x => x is NurbsCurve))
        //    {
        //        var startIndex = vertices.Count();
        //        for (int i = 0; i < curve.NumberOfPoints; i++)
        //        {
        //            vertices.Add(new Vector3d(curve.GetCV(i)));
        //            normals.Add(new Vector3d(0, 0, 1));
        //        }
        //        for (int i = startIndex; i < startIndex + curve.NumberOfPoints - 1; i++)
        //        {
        //            indices.AddRange(new int[] { i, i + 1 });
        //        }

        //        if (curve.NumberOfPoints > 2)
        //        {
        //            startIndex = vertices.Count();
        //            foreach (var v in curve.ToLines(1000))
        //            {
        //                vertices.Add(new Vector3d(v));
        //                normals.Add(new Vector3d(0, 0, 1));
        //            }
        //            for (int i = startIndex; i < startIndex + 1000; i++)
        //            {
        //                indices.AddRange(new int[] { i, i + 1 });
        //            }
        //        }
        //    }


        //}

    }







}
