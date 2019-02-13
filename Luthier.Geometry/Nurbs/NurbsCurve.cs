﻿
using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Geometry.Nurbs
{

    [Serializable]
    public class NurbsCurve
    {

        [XmlElement]
        public int _dimension;
        [XmlElement]
        public int _order;
        [XmlElement]
        public bool _isRational;
        [XmlElement]
        public int _cvSize;
        [XmlArray]
        public double[] knot;
        [XmlArray]
        public double[] cvDataBlock;
        [XmlElement]
        public int _cvCount;

        private NurbsCurveBulkEvaluator _bulkEvaluator;


        public NurbsCurve() { }
        public NurbsCurve(int dimension, bool isRational, int order, int cvCount)
        {
            _dimension = dimension;
            _isRational = isRational;
            _order = order;
            _cvSize = (isRational) ? dimension + 1 : dimension;
            _cvCount = cvCount;

            knot = new double[_cvCount + _order];
            for (int i = 0; i < knot.Length; i++) knot[i] = i;

            cvDataBlock = new double[_cvSize * _cvCount];
        }


        public double[] GetCV(int IX)
        {
            double[] cv = new double[_cvSize];
            for (int i = 0; i < _cvSize; i++) cv[i] = cvDataBlock[IX + i * _cvCount];
            return cv;
        }

        public void SetCV(int IX, double[] cv)
        {
            for (int i = 0; i < _cvSize; i++) cvDataBlock[IX + i * _cvCount] = cv[i];
        }
        public double GetKnot(int IX)
        {
            return knot[IX];
        }
        public int GetDegree() => _order - 1;
        public int NumberOfPoints { get => _cvCount; }

        public Interval Domain
        {
            get => new Interval(knot[_order - 1], knot[knot.Length - _order]);
        }

        public double[] Evaluate(double t)
        {
            double[] p = new double[_cvSize];
            Evaluate(t, p);
            return p;
        }

        public void Evaluate(double t, double[] point)
        {
            if (_order == 3)
            {
                int[] indices = new int[3];
                double[] values = new double[3];
                Algorithm.BasisFunction_EvaluateAllNonZero_DegreeTwo(knot, t, ref values, ref indices);

                Algorithm.Curve_EvaluateGivenBasisFunctions(_order - 1, _dimension, indices[0], values, cvDataBlock, 1, _cvCount, ref point);

            }
            else throw new NotImplementedException();
        }

        public void SetEvaluationPoints(double[] t)
        {
            _bulkEvaluator = new NurbsCurveBulkEvaluator(this, t);
        }
        public double[] RecalculateAtEvaluationPoints()
        {
            return _bulkEvaluator.Evaluate();
        }

        public double[] EvaluateDerivative(int derivative, double t)
        {
            double[] p = new double[_cvSize];
            EvaluateDerivative(derivative, t, p);
            return p;
        }

        public void EvaluateDerivative(int derivative, double t, double[] point)
        {
            int knotIX = Algorithm.Find_Knot_Span(_order - 1, knot, t);
            int cvIX = knotIX - _order + 1;
            for (int i = 0; i < _cvSize; i++)
            {
                if(derivative == 1)
                {
                    point[i] = Geometry.Nurbs.Algorithm.CurveSpan_EvaluateFirstDerivative_Deboor(_order - 1, knotIX, ref knot, ref cvDataBlock, i * _cvCount + cvIX, cvStride: 1, t: t);
                }
                else
                {
                    point[i] = Geometry.Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(derivative, _order - 1, knotIX, ref knot, ref cvDataBlock, i * _cvCount + cvIX, cvStride: 1, t: t);
                }
            }
        }

        public double[] EvaluateAllDerivatives(double t)
        {
            double[] values = new double[_dimension * _order];
            EvaluateAllDerivatives(t, values);
            return values;
        }
        public void EvaluateAllDerivatives(double t, double[] values)
        {
            if(_order == 3)
            {
                int[] indices = new int[3];
                double[] basis = new double[9];
                Algorithm.BasisFunction_EvaluateAllNonZero_AllDerivatives_DegreeTwo(knot, t, ref basis, ref indices);

                for (int i = 0; i < _cvSize; i++)
                {
                    values[i] = 0;
                    values[i + _dimension] = 0;
                    values[i + 2 * _dimension] = 0;

                    for (int j = 0; j < _order; j++)
                    {
                        int cvIX = indices[j] + i * _cvCount;

                        values[i] += cvDataBlock[cvIX] * basis[j];
                        values[i + _dimension] += cvDataBlock[cvIX] * basis[j + _order];
                        values[i + 2 * _dimension] += cvDataBlock[cvIX] * basis[j + 2 * _order];
                    }
                }
                
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        public void EvaluateAllDerivatives(double[] basis, int[] indices, ref double[] d0, ref double[] d1, ref double[] d2)
        {
            if (_order == 3)
            {
                for (int i = 0; i < _cvSize; i++)
                {
                    d0[i] = 0;
                    d1[i] = 0;
                    d2[i] = 0;

                    for (int j = 0; j < _order; j++)
                    {
                        int cvIX = indices[j] + i * _cvCount;

                        d0[i] += cvDataBlock[cvIX] * basis[j];
                        d1[i] += cvDataBlock[cvIX] * basis[j + _order];
                        d2[i] += cvDataBlock[cvIX] * basis[j + 2 * _order];
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }

        }



        /// <summary>
        /// returns a vector from point on curve C(t) to the centre of its osculating circle
        /// </summary>
        /// <param name="t"></param>
        /// <param name="result"></param>
        public void CentreOfCurvature(double t, double[] result)
        {
            if (_dimension == 2)
            {
                double[] d1 = EvaluateDerivative(1, t);
                double[] d2 = EvaluateDerivative(2, t);

                Curvature.CentreOfCurvature_TwoDimensions(d1, d2, ref result);

            }
            else if(_dimension == 3)
            {
                double[] d1 = EvaluateDerivative(1, t);
                double[] d2 = EvaluateDerivative(2, t);

                Curvature.CentreOfCurvature_ThreeDimensions(d1, d2, ref result);

            }
            else throw new NotImplementedException();
        }

        public double[] CentreOfCurvature(double t)
        {
            double[] result = new double[_dimension];
            CentreOfCurvature(t, result);
            return result;
        }


        public List<double[]> ToLines(int numberOfLines) => ToLines(numberOfLines, double.MinValue, double.MaxValue);
        public List<double[]> ToLines(int numberOfLines, double from, double to)
        {
            from = Math.Max(from, Domain.Min);
            to = Math.Min(to, Domain.Max);
            var result = new List<double[]>();

            for (int i = 0; i <= numberOfLines; i++)
            {
                var t = ((double)i / numberOfLines) * to + (1 - (double)i / numberOfLines) * from;
                var point = Evaluate(t);
                if (point != null) result.Add(point);
            }
            return result;
        }

        public void InsertKnots(List<double> knots)
        {
            //var newKnot = new Knot { p = knot.p, data = new List<double>(knot.data) };
            //newKnot.data.AddRange(knots);
            //newKnot.data.Sort();
            //points = BSpline.Algorithm.olso_insertion(points, knot.p, knot.data, newKnot.data);
            //knot = newKnot;
        }

        public void CloseFront()
        {
            //var newknot = new List<double>();
            //var minParam = knot.minParam;
            //for (int i = 0; i < knot.Continuity(minParam); i++) newknot.Add(minParam);
            //InsertKnots(newknot);
            //points = points.GetRange(newknot.Count, points.Count - newknot.Count);
            //knot.data = knot.data.GetRange(newknot.Count, knot.data.Count - newknot.Count);
        }

        public void CloseBack()
        {
            //var newknots = new List<double>();
            //var maxParam = knot.maxParam;
            //for (int i = 0; i < knot.Continuity(maxParam); i++) newknots.Add(maxParam);
            //InsertKnots(newknots);
            //points = points.GetRange(0, points.Count - newknots.Count);
            //knot.data = knot.data.GetRange(0, knot.data.Count - newknots.Count);
        }


        public NurbsCurve DeepCopy()
        {
            var copy = new NurbsCurve(_dimension, _isRational, _order, _cvCount);
            Array.Copy(cvDataBlock, copy.cvDataBlock, cvDataBlock.Length);
            Array.Copy(knot, copy.knot, knot.Length);
            return copy;
        }



        public void ExtendFront(double[] cv)
        {
            if (cv.Length != _cvSize) return;

            var newKnot = new double[knot.Length + 1];
            Array.Copy(knot, 0, newKnot, 1, knot.Length);
            newKnot[0] = 2 * newKnot[1] - newKnot[2];

            var newCvDataBlock = new double[cvDataBlock.Length + _cvSize];
            Array.Copy(cvDataBlock, 0, newCvDataBlock, _cvSize, cvDataBlock.Length);
            Array.Copy(cv, 0, newCvDataBlock, 0, _cvSize);

            _cvCount++;

            cvDataBlock = newCvDataBlock;
            knot = newKnot;
        }

        public void ExtendBack(double[] cv)
        {
            if (cv.Length != _cvSize) return;

            var newKnot = new double[knot.Length + 1];
            Array.Copy(knot, 0, newKnot, 0, knot.Length);
            newKnot[newKnot.Length - 1] = 2 * newKnot[newKnot.Length - 2] - newKnot[newKnot.Length - 3];

            var newCvDataBlock = new double[cvDataBlock.Length + _cvSize];
            for (int i = 0; i < _cvCount; i++)
            {
                for (int j = 0; j < _cvSize; j++)
                {
                    newCvDataBlock[i + j * (_cvCount + 1)] = cvDataBlock[i + j * _cvCount];
                }
            }
            for (int j = 0; j < _cvSize; j++)
            {
                newCvDataBlock[_cvCount + j * (_cvCount + 1)] = cv[j];
            }

            _cvCount++;

            cvDataBlock = newCvDataBlock;
            knot = newKnot;
        }


        public NurbsCurveNearestPointResult NearestSquaredDistance(PointCloud cloud, int numberOfFootPoints)
        {
            return NearestSquaredDistanceBinaryTreeExact(cloud, numberOfFootPoints);
        }

        public NurbsCurveNearestPointResult NearestSquaredDistanceLinear(PointCloud cloud, int numberOfFootPoints)
        {
            var sw = new System.Diagnostics.Stopwatch();

            var result = new NurbsCurveNearestPointResult
            {
                Distances = new double[cloud.PointCount],
                Coordinates = new double[cloud.PointCount * cloud.Dimension],
            };

            sw.Restart();
            var footPoints = ToLines(numberOfFootPoints);
            long t1 = sw.ElapsedMilliseconds;

            sw.Restart();

            double[] cp = new double[cloud.Dimension];

            for (int i = 0; i < cloud.PointCount; i++)
            {
                cloud.GetPointFast(i, cp);

                double distance = double.MaxValue;
                int footPointIndex = 0;
                for (int j = 0; j < footPoints.Count; j++)
                {
                    double[] fp = footPoints[j];

                    double dx = fp[0] - cp[0];
                    double dy = fp[1] - cp[1];

                    double d2 = dx * dx + dy * dy;
                    if (d2 < distance)
                    {
                        distance = d2;
                        footPointIndex = j;
                    }
                }
                result.Distances[i] = distance;
                Array.Copy(footPoints[footPointIndex], 0, result.Coordinates, i * cloud.Dimension, cloud.Dimension);
            }
            long t3 = sw.ElapsedMilliseconds;

            result.Messages = new List<string>();
            result.Messages.Add($"Time to create {numberOfFootPoints} footpoints: {t1}ms");
            result.Messages.Add($"Time to calculate distances for {cloud.PointCount} cloudpoints: {t3}ms");

            return result;
        }

        public NurbsCurveNearestPointResult NearestSquaredDistanceBinaryTree(PointCloud cloud, int numberOfFootPoints)
        {
            var sw = new System.Diagnostics.Stopwatch();

            var result = new NurbsCurveNearestPointResult
            {
                Distances = new double[cloud.PointCount],
                Coordinates = new double[cloud.PointCount * cloud.Dimension],
            };

            sw.Restart();
            
            var tree = new NurbsCurveBinaryTree(this, numberOfFootPoints);
            long t1 = sw.ElapsedMilliseconds;

            sw.Restart();
            double[] cp = new double[cloud.Dimension];

            for (int i = 0; i < cloud.PointCount; i++)
            {
                cloud.GetPointFast(i, cp);

                int parentIndex = tree.Nodes.Count - 1;
                double distance = tree.Nodes[parentIndex].DistanceSquared(cp[0], cp[1]) + tree.Nodes[parentIndex].Radius;

                var parentList = new List<NodeDistance>() { new NodeDistance { Index = parentIndex, Distance = distance } };

                double smallestMax = double.MaxValue;

                while (parentList.Count > 0)
                {
                    var childrenList = new List<NodeDistance>();

                    //find distances of children of current nodes
                    foreach (var parent in parentList)
                    {
                        if (parent.Distance < smallestMax)
                        {
                            int childIndex1 = tree.Nodes[parent.Index].ChildNodeStartIndex;
                            double distance1 = tree.Nodes[childIndex1].DistanceSquared(cp[0], cp[1]);
                            double max1 = distance1 + tree.Nodes[childIndex1].Radius;
                            double min1 = distance1 - tree.Nodes[childIndex1].Radius;

                            if (max1 < smallestMax)
                                smallestMax = max1;

                            if (min1 < smallestMax)
                                childrenList.Add(new NodeDistance { Index = childIndex1, Distance = min1 });

                            int childIndex2 = tree.Nodes[parent.Index].ChildNodeEndIndex;
                            double distance2 = tree.Nodes[childIndex2].DistanceSquared(cp[0], cp[1]);
                            double max2 = distance2 + tree.Nodes[childIndex2].Radius;
                            double min2 = distance2 - tree.Nodes[childIndex2].Radius;

                            if (max2 < smallestMax)
                                smallestMax = max2;

                            if (min2 < smallestMax)
                                childrenList.Add(new NodeDistance { Index = childIndex2, Distance = min2 });
                        }
                    }

                    parentList = childrenList;
                }

                result.Distances[i] = smallestMax;
            }
            long t3 = sw.ElapsedMilliseconds;

            result.Messages = new List<string>();
            result.Messages.Add($"Time to create NurbsCurveBinaryTree with {tree.FootPointCount} footpoints: {t1}ms");
            result.Messages.Add($"Time to calculate distances for {cloud.PointCount} cloudpoints: {t3}ms");

            return result;
        }

        public NurbsCurveNearestPointResult NearestSquaredDistanceBinaryTreeExact(PointCloud cloud, int numberOfFootPoints)
        {
            var sw = new System.Diagnostics.Stopwatch();

            var result = new NurbsCurveNearestPointResult
            {
                Distances = new double[cloud.PointCount],
                Parameters = new double[cloud.PointCount],
                Coordinates = new double[cloud.PointCount * cloud.Dimension],
            };

            sw.Restart();

            var tree = new NurbsCurveBinaryTree(this, numberOfFootPoints);
            long t1 = sw.ElapsedMilliseconds;

            sw.Restart();
            double[] cp = new double[cloud.Dimension];

            for (int i = 0; i < cloud.PointCount; i++)
            {
                cloud.GetPointFast(i, cp);

                int parentIndex = tree.Nodes.Count - 1;
                double distance = tree.Nodes[parentIndex].DistanceSquared(cp[0], cp[1]) + tree.Nodes[parentIndex].Radius;

                var parentList = new List<NodeDistance>() { new NodeDistance { Index = parentIndex, Distance = distance } };

                double smallestMax = double.MaxValue;
                int nearestNodeIndex = 0;

                while (parentList.Count > 0)
                {
                    var childrenList = new List<NodeDistance>();

                    //find distances of children of current nodes
                    foreach (var parent in parentList)
                    {
                        if (parent.Distance < smallestMax)
                        {
                            int childIndex1 = tree.Nodes[parent.Index].ChildNodeStartIndex;
                            double distance1 = tree.Nodes[childIndex1].DistanceSquared(cp[0], cp[1]);
                            double max1 = distance1 + tree.Nodes[childIndex1].Radius;
                            double min1 = distance1 - tree.Nodes[childIndex1].Radius;

                            if (max1 < smallestMax)
                            {
                                smallestMax = max1;
                                nearestNodeIndex = childIndex1;
                            }

                            if (min1 < smallestMax)
                                childrenList.Add(new NodeDistance { Index = childIndex1, Distance = min1 });

                            int childIndex2 = tree.Nodes[parent.Index].ChildNodeEndIndex;
                            double distance2 = tree.Nodes[childIndex2].DistanceSquared(cp[0], cp[1]);
                            double max2 = distance2 + tree.Nodes[childIndex2].Radius;
                            double min2 = distance2 - tree.Nodes[childIndex2].Radius;

                            if (max2 < smallestMax)
                            {
                                smallestMax = max2;
                                nearestNodeIndex = childIndex2;
                            }

                            if (min2 < smallestMax)
                                childrenList.Add(new NodeDistance { Index = childIndex2, Distance = min2 });
                        }
                    }

                    parentList = childrenList;
                }

                NurbsCurveNearestPoint np = new NurbsCurveNearestPoint(this, cp);
                double[] c0 = new double[_dimension * _order];
                double[] c1 = new double[_dimension];
                double[] c2 = new double[_dimension];

                //double t = np.NearestSquaredDistanceNewtonRaphson(tree.Nodes[nearestNodeIndex].Minparameter - 0.1, tree.Nodes[nearestNodeIndex].MaxParameter + 0.1,ref c0,ref c1,ref c2);

                double t = np.FindRoot(tree.Nodes[nearestNodeIndex].Minparameter - 0.1, tree.Nodes[nearestNodeIndex].MaxParameter + 0.1, ref c0);

                result.Distances[i] = (c0[0] - cp[0]) * (c0[0] - cp[0]) + (c0[1] - cp[1]) * (c0[1] - cp[1]);
                result.Parameters[i] = t;
                result.Coordinates[i * cloud.Dimension] = c0[0];
                result.Coordinates[i * cloud.Dimension + 1] = c0[1];
            }
            long t3 = sw.ElapsedMilliseconds;

            result.Messages = new List<string>();
            result.Messages.Add($"Time to create NurbsCurveBinaryTree with {tree.FootPointCount} footpoints: {t1}ms");
            result.Messages.Add($"Time to calculate distances for {cloud.PointCount} cloudpoints: {t3}ms");

            return result;
        }


        public NurbsCurve Scale(double scaleFactor)
        {
            var result = DeepCopy();
            for (int i = 0; i < result.cvDataBlock.Length; i++)
                result.cvDataBlock[i] *= scaleFactor;

            return result;
        }
    }



    public class NurbsCurveNearestPointResult
    {
        public double[] Parameters;
        public double[] Distances;
        public double[] Coordinates;
        public List<string> Messages;
    }
}
