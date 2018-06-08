using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Luthier.Geometry.BSpline
{
    public class NurbsSurface
    {

        private int dimension;
        private int[] order;
        private bool isRational;
        private int cvSize;


        public double[][] knot;
        public double[] cvDataBlock; // cv data is stored in column-major U-V order, and finally in dimension order
        private int[] cvCount;

        public NurbsSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            this.dimension = dimension;
            this.isRational = false;
            this.order = new int[] { order0, order1 };
            this.cvCount = new int[] { cv_count0, cv_count1 };

            Initialise();
        }

        private void Initialise()
        {
            knot = new double[][] 
            {
                new double[cvCount[0] + order[0] - 2],
                new double[cvCount[1] + order[1] - 2],
            };
            cvSize = (isRational) ? dimension + 1 : dimension;
            cvDataBlock = new double[cvSize * cvCount[0] * cvCount[1]];
        }

        public int CVSize => cvSize;
        public int Dimension => dimension;
        
        public double[] Evaluate(double u, double v)
        {
            var result = new double[dimension];

            int knotIU = Algorithm.Find_Knot_Span(order[0] - 1, knot[0], u);
            int knotIV = Algorithm.Find_Knot_Span(order[1] - 1, knot[1], v);
            int cvIX = (knotIU - order[0] + 2) * cvCount[1] + knotIV - order[1] + 2;
            int cvStrideU = cvCount[1];
            int cvStrideV = 1;

            for(int i = 0; i < dimension; i++)
            {
                result[i] = Algorithm.Evaluate_SurfaceSpan_Deboor(order[0], order[1], knotIU, knotIV, ref knot[0], ref knot[1], ref cvDataBlock, cvIX, cvStrideU, cvStrideV, u, v);
                cvIX += cvCount[0] * cvCount[1];
            }
            return result;
        }

        public double[] EvaluateNormal(double u, double v)
        {
            var result = new double[dimension];

            double delta = 0.0001;
            var p0 = Evaluate(u, v);
            var pU = Evaluate(u + delta, v);
            var pV = Evaluate(u, v + delta);

            var u1 = pU[0] - p0[0];
            var u2 = pU[1] - p0[1];
            var u3 = pU[2] - p0[2];

            var v1 = pV[0] - p0[0];
            var v2 = pV[1] - p0[1];
            var v3 = pV[2] - p0[2];

            result[0] = -u2 * v3 + u3 * v2;
            result[1] = -u3 * v1 + u1 * v3;
            result[2] = -u1 * v2 + u2 * v1;

            return result;
         }

        public int CVCount(int direction) => cvCount[direction];
        public void SetCV(int IU, int IV, double[] point)
        {
            int cvIX = (IU * cvCount[1] + IV);
            int stride = cvCount[0] * cvCount[1];
            for(int i=0; i < cvSize; i++)
            {
                cvDataBlock[cvIX] = point[i];
                cvIX += stride;
            }
        }
        public double[] GetCV(int IU, int IV)
        {
            double[] point = new double[cvSize];
            int cvIX = (IU * cvCount[1] + IV);
            int stride = cvCount[0] * cvCount[1];
            for (int i = 0; i < cvSize; i++)
            {
                point[i] = cvDataBlock[cvIX];
                cvIX += stride;
            }
            return point;
        }
        public void SetKnot(int direction, int IX, double knot_value)
        {
            knot[direction][IX] = knot_value;
        }
        public double GetKnot(int direction, int IX)
        {
            return knot[direction][IX];
        }
        public int KnotCount(int direction) { return knot[direction].Length; }

        public Interval Domain(int direction)
        {
            double min = knot[direction][order[direction] - 2];
            double max = knot[direction][knot[direction].Length - order[direction] + 1];
            return new Interval(min, max);
        }

        //public bool InsertKnot(int direction, double knot_value, int knot_multiplicity) { return Convert.ToBoolean(surface.InsertKnot(direction, knot_value, knot_multiplicity)); }
        //public bool IncreaseDegree(int direction, int desired_degree) { return Convert.ToBoolean(surface.IncreaseDegree(direction, desired_degree)); }

        public NurbsSurface Copy()
        {
            var result = new NurbsSurface(dimension, isRational, order[0], order[1], cvCount[0], cvCount[1]);
            Array.Copy(cvDataBlock, result.cvDataBlock, cvDataBlock.Length);
            Array.Copy(knot[0], result.knot[0], knot[0].Length);
            Array.Copy(knot[1], result.knot[1], knot[1].Length);
            return result;
        }
    }




}
