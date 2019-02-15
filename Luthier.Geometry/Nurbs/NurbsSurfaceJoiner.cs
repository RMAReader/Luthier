using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{

    public enum EnumSurfaceEdge
    {
        North, South, East, West
    }

    public class NurbsSurfaceJoiner
    {


        public static NurbsSurface CreateBridgingSurface(NurbsSurfaceEdge edge1, NurbsSurfaceEdge edge2)
        {
            TryGetCompatibleEdges(edge1, edge2, out NurbsSurfaceEdge top, out NurbsSurfaceEdge bottom);
           
            int cvCountbridge = 3;

            var result = new NurbsSurface(top.Surface.Dimension, 
                top.Surface.IsRational, 
                top.Order, 
                top.Order, 
                top.CvCount, 
                cvCountbridge);

            Array.Copy(top.Knot, result.knotArray0, top.Knot.Length);
            result.knotArray1 = Knot.CreateUniformClosed(2, cvCountbridge + top.Order).data.ToArray();

            for(int i = 0; i < result.CvCount0; i++)
            {
                var topCv = top.GetCV(i);
                var bottomCv = bottom.GetCV(i);
                var midCv = topCv.Add(bottomCv).Multiply(0.5);

                result.SetCV(i, 0, bottomCv);
                result.SetCV(i,1, midCv);
                result.SetCV(i,2, topCv);
            }


            return result;
        }



        private void VerifyKnotIsNormalised(double[] knot)
        {
            if (knot[0] != 0 || knot[knot.Length - 1] != 1)
                throw new ArgumentException();
        }

        private static bool TryGetCompatibleEdges(NurbsSurfaceEdge e1, NurbsSurfaceEdge e2, out NurbsSurfaceEdge c1, out NurbsSurfaceEdge c2)
        {
            var newKnots1 = new List<double>();
            var newKnots2 = new List<double>();

            int i = 0;
            int j = 0;
            while (i < e1.Knot.Length || j < e2.Knot.Length)
            {
                if (e1.Knot[i] == e2.Knot[j])
                {
                    i++;
                    j++;
                }
                else if (e1.Knot[i] < e2.Knot[j])
                {
                    newKnots2.Add(e1.Knot[i]);
                    i++;
                }
                else if (e1.Knot[i] > e2.Knot[j])
                {
                    newKnots1.Add(e2.Knot[j]);
                    j++;
                }
            }

            c1 = e1.DeepCopy();
            c1.InsertKnots(newKnots1.ToArray());

            c2 = e2.DeepCopy();
            c2.InsertKnots(newKnots2.ToArray());

            return true;
        }


    }


    public class NurbsSurfaceEdge
    {
        public NurbsSurface Surface { get; private set; }

        public EnumSurfaceEdge Edge { get; private set; }

        public int Direction => (Edge == EnumSurfaceEdge.North || Edge == EnumSurfaceEdge.South) ? 1 : 0;

        public double[] Knot => Direction == 0 ? Surface.knotArray0 : Surface.knotArray1;

        public int Order => Direction == 0 ? Surface.Order0 : Surface.Order1;

        public int CvCount => Surface.controlPoints.CvCount[Direction];
        
        public double[] GetCV(int i)
        {
            if(Edge == EnumSurfaceEdge.North)
                return Surface.GetCV(0, i);

            if (Edge == EnumSurfaceEdge.South)
                return Surface.GetCV(CvCount - 1, i);

            if (Edge == EnumSurfaceEdge.West)
                return Surface.GetCV(i, 0);

            if (Edge == EnumSurfaceEdge.East)
                return Surface.GetCV(i, CvCount - 1);

            return null;
        }

        public void InsertKnots(double[] newKnots)
        {
            Surface = Surface.InsertKnot(Direction, newKnots);
        }

        public NurbsSurfaceEdge DeepCopy()
        {
            return new NurbsSurfaceEdge(Surface.DeepCopy(), Edge);
        }





        public NurbsSurfaceEdge(NurbsSurface surface, EnumSurfaceEdge edge)
        {
            Surface = surface;
            Edge = edge;
        }
    }
}
