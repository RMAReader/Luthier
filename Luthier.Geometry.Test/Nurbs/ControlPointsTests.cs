using Luthier.Geometry.Nurbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test.Nurbs
{
    [TestCategory("CorrectnessTests")]
    [TestClass]
    public class ControlPointsTests
    {

        [TestMethod]
        public void ControlPoints_SetCV_GetCV()
        {
            var controlPoints = new List<Tuple<int[], double[]>>
            {
                new Tuple<int[], double[]>(new int[] { 0, 0 }, new double[] { 1.0, 2.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 1 }, new double[] { 3.0, 4.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 2 }, new double[] { 5.0, 6.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 0 }, new double[] { 7.0, 8.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 1 }, new double[] { 9.0, 10.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 2 }, new double[] { 11.0, 12.0 })
            };

            var contolPoints = new ControlPoints(dimension: 2, cvCount: new int[] { 2, 3 });
            foreach (var cp in controlPoints)
            {
                contolPoints.SetCV(cp.Item2, cp.Item1);
            }

            foreach (var expected in controlPoints)
            {
                var actual = new double[2];

                contolPoints.GetCV(actual, expected.Item1);

                Assert.AreEqual(expected.Item2[0], actual[0]);
                Assert.AreEqual(expected.Item2[1], actual[1]);
            }
        }

        [TestMethod]
        public void ControlPoints_GetDataIndex()
        {
            var controlPoints = new List<Tuple<int[], double[]>>
            {
                new Tuple<int[], double[]>(new int[] { 0, 0 }, new double[] { 1.0, 2.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 1 }, new double[] { 3.0, 4.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 2 }, new double[] { 5.0, 6.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 0 }, new double[] { 7.0, 8.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 1 }, new double[] { 9.0, 10.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 2 }, new double[] { 11.0, 12.0 })
            };

            var contolPoints = new ControlPoints(dimension: 2, cvCount: new int[] { 2, 3 });
            foreach (var cp in controlPoints)
            {
                contolPoints.SetCV(cp.Item2, cp.Item1);
            }

            foreach (var expected in controlPoints)
            {
                for(int dimension = 0; dimension < 2; dimension++)
                {
                    int index = contolPoints.GetDataIndex(dimension, expected.Item1);

                    Assert.AreEqual(expected.Item2[dimension], contolPoints.Data[index]);
                }
            }

        }

        [TestMethod]
        public void ControlPoints_EnumerateControlPoints()
        {
            var cvs = new List<Tuple<int[], double[]>>
            {
                new Tuple<int[], double[]>(new int[] { 0, 0 }, new double[] { 1.0, 2.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 1 }, new double[] { 3.0, 4.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 2 }, new double[] { 5.0, 6.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 0 }, new double[] { 7.0, 8.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 1 }, new double[] { 9.0, 10.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 2 }, new double[] { 11.0, 12.0 })
            };

            var controlPoints = new ControlPoints(dimension: 2, cvCount: new int[] { 2, 3 });
            foreach (var cp in cvs)
            {
                controlPoints.SetCV(cp.Item2, cp.Item1);
            }

            var points = controlPoints.EnumerateControlPoints().ToList();

            Assert.AreEqual(cvs.Count, points.Count);
            foreach(var p in cvs)
            {
                var point = points.First(x => x.Item1[0] == p.Item1[0] && x.Item1[1] == p.Item1[1]);
                Assert.AreEqual(p.Item2[0], point.Item2[0]);
                Assert.AreEqual(p.Item2[1], point.Item2[1]);
            }
        }

        [TestMethod]
        public void ControlPoints_Transform()
        {
            var cvs = new List<Tuple<int[], double[]>>
            {
                new Tuple<int[], double[]>(new int[] { 0, 0 }, new double[] { 1.0, 2.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 1 }, new double[] { 3.0, 4.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 2 }, new double[] { 5.0, 6.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 0 }, new double[] { 7.0, 8.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 1 }, new double[] { 9.0, 10.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 2 }, new double[] { 11.0, 12.0 })
            };

            var controlPoints = new ControlPoints(dimension: 2, cvCount: new int[] { 2, 3 });
            foreach (var cp in cvs)
            {
                controlPoints.SetCV(cp.Item2, cp.Item1);
            }

            var transform = new double[,] { { 1, 0 }, { 0, 1 }, { 0.5, 0.5 } };

            var transformedCp = controlPoints.Transform(transform);
        }

        [TestMethod]
        public void ControlPoints_Apply()
        {
            var cvs = new List<Tuple<int[], double[]>>
            {
                new Tuple<int[], double[]>(new int[] { 0, 0 }, new double[] { 1.0, 2.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 1 }, new double[] { 3.0, 4.0 }),
                new Tuple<int[], double[]>(new int[] { 0, 2 }, new double[] { 5.0, 6.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 0 }, new double[] { 7.0, 8.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 1 }, new double[] { 9.0, 10.0 }),
                new Tuple<int[], double[]>(new int[] { 1, 2 }, new double[] { 11.0, 12.0 })
            };

            var controlPoints = new ControlPoints(dimension: 2, cvCount: new int[] { 2, 3 });
            foreach (var cp in cvs)
            {
                controlPoints.SetCV(cp.Item2, cp.Item1);
            }

            var rotation = new double[,] {{ 0, 1 }};
            var translation = new double[] { 1 };

            Func<double[], double[]> f = (double[] cv) => { var p = rotation.DotProduct(cv); return p.Add(translation); };

            var transformedCp = controlPoints.Apply(f).EnumerateControlPoints().ToList();
        }
    }
}
