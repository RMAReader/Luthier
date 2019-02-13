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


        public NurbsSurface CreateBridgingSurface(NurbsSurface s1, EnumSurfaceEdge edge1, NurbsSurface s2, EnumSurfaceEdge edge2)
        {

            var knot1 = GetKnot(s1, edge1);
            var knot2 = GetKnot(s2, edge2);

            VerifyKnotIsNormalised(knot1);
            VerifyKnotIsNormalised(knot2);

            var newKnots1 = new List<double>();
            var newKnots2 = new List<double>();

            int i = 0;
            int j = 0;
            while(i < knot1.Length || j < knot2.Length)
            {
                if(knot1[i] == knot2[j])
                {
                    i++;
                    j++;
                }
                else if(knot1[i] < knot2[j])
                {
                    newKnots2.Add(knot1[i]);
                    i++;
                }
                else if(knot1[i] > knot2[j])
                {
                    newKnots1.Add(knot2[j]);
                    j++;
                }
            }

            var s1Compatible = s1.InsertKnot(Direction(edge1), newKnots1.ToArray());
            var s2Compatible = s2.InsertKnot(Direction(edge2), newKnots2.ToArray());

            int cvCountbridge = 3;

            var result = new NurbsSurface(s1.Dimension, s1.IsRational, s1.Order0, s1.Order1, s1Compatible.controlPoints.CvCount[Direction(edge1)], cvCountbridge);

            double[] cv = new double[s1.Dimension];
            for(i = 0; i < result.CvCount0; i++)
            {
                s1Compatible.controlPoints.
                result.controlPoints.SetCV()
            }


            return result;
        }



        private void VerifyKnotIsNormalised(double[] knot)
        {
            if (knot[0] != 0 || knot[knot.Length - 1] != 1)
                throw new ArgumentException();
        }



        private double[] GetKnot(NurbsSurface s, EnumSurfaceEdge edge)
        {
            int d = Direction(edge);

            return d == 0 ? s.knotArray0 :
                d == 1 ? s.knotArray1 :
                null;

        }


        private int Direction(EnumSurfaceEdge edge)
        {
            if (edge == EnumSurfaceEdge.North || edge == EnumSurfaceEdge.South)
            {
                return 0;
            }
            if (edge == EnumSurfaceEdge.East || edge == EnumSurfaceEdge.West)
            {
                return 1;
            }
            return -1;
        }
    }
}
