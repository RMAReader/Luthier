using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using opennurbs_CLI;

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


    }





    public class NurbsSurface_OpenNurbs : IDisposable
    {
        private opennurbs_CLI.NurbsSurface surface;

        public NurbsSurface_OpenNurbs(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            surface = new opennurbs_CLI.NurbsSurface(dimension, Convert.ToInt32(bIsRational), order0, order1, cv_count0, cv_count1);
        }

        public int Dimension { get { return surface.Dimension; } }
        public int Degree(int direction) { return surface.Degree(direction); }
        public int Order(int direction) { return surface.Order(direction); }
        public int CVCount { get { return surface.CVCount; } }
        public int KnotCount(int direction) { return surface.KnotCount(direction); }
        public Interval Domain(int direction) { return new Interval(surface.Domain(direction).Min, surface.Domain(direction).Max); }

        public double[] Evaluate(double u, double v, int nderiv)
        {
            return surface.Evaluate(u, v, nderiv);
        }

        public double[] EvaluateNormal(double u, double v)
        {
            return surface.EvNormal(u, v);
        }

        public bool SetCV(int IU, int IV, int style, double[] point)
        {
            return Convert.ToBoolean(surface.SetCV(IU, IV, 3, point));
        }
        public double[] GetCV(int IU, int IV, int style)
        {
            return surface.GetCV(IU, IV, style);
        }
        public bool SetKnot(int direction, int IX, double knot_value) { return Convert.ToBoolean(surface.SetKnot(direction, IX, knot_value)); }
        public double GetKnot(int direction, int IX) { return surface.GetKnot(direction, IX); }

        public bool InsertKnot(int direction, double knot_value, int knot_multiplicity) { return Convert.ToBoolean(surface.InsertKnot(direction, knot_value, knot_multiplicity)); }
        public bool IncreaseDegree(int direction, int desired_degree) { return Convert.ToBoolean(surface.IncreaseDegree(direction, desired_degree)); }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                surface.Dispose();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~NurbsSurface_OpenNurbs()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion



    }
}
