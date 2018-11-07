using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
 
    /// <summary>
    /// Class representing a binary tree hierarchy for nurbs curve, where the curve is divided in to spans of decreasing length. 
    /// </summary>
    public class NurbsCurveBinaryTree
    {
        public const uint MaximumFootPointCount = 1048576; // 2^20;
        private readonly NurbsCurve _curve;
        public int Dimension => _curve._dimension;
        public int FootPointCount { get; private set; }
        public List<TreeNode> Nodes;

        public NurbsCurveBinaryTree(NurbsCurve curve, int minimumNumberOfPoints)
        {
            _curve = curve;

            FootPointCount = GetNumberOfPoints(minimumNumberOfPoints);

            Nodes = new List<TreeNode>(2 * FootPointCount);

            //1. create lowest level by evaluatings points on nurbs curve
            int divisor = FootPointCount - 1;
            double[] p = new double[_curve._dimension];
            for (int i = 0; i < FootPointCount; i++)
            {
                var t = ((double)i / divisor) * _curve.Domain.Max + (1 - (double)i / divisor) * _curve.Domain.Min;
                curve.Evaluate(t, p);
                Nodes.Add(new TreeNode
                {
                    Minparameter = t,
                    MaxParameter = t,
                    Radius = 0,
                    CentreX = p[0],
                    CentreY = p[1],
                    ChildNodeStartIndex = -1,
                    ChildNodeEndIndex = -1
                });
            }

            //2. create higher levels by combining spherical regions of lower levels
            int childStartIndex = 0;
            int childEndIndex = Nodes.Count;

            while (childStartIndex + 1 < childEndIndex)
            {
                for (int i = childStartIndex; i < childEndIndex; i += 2)
                {
                    if (i + 1 < childEndIndex)
                    {
                        Nodes.Add(GetParentNode(i));
                    }
                    else
                    {
                        Nodes.Add(Nodes[i]);
                    }
                }

                childStartIndex = childEndIndex;
                childEndIndex = Nodes.Count;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TreeNode GetParentNode(int i)
        {
            //set centre to midpoint between centres of children
            int j = i + 1;
            double distance = Nodes[i].DistanceSquared(Nodes[j].CentreX, Nodes[j].CentreY);

            TreeNode result =  new TreeNode();
            result.ChildNodeStartIndex = i;
            result.ChildNodeEndIndex = j;
            result.Minparameter = Nodes[i].Minparameter;
            result.MaxParameter = Nodes[j].Minparameter;
            result.CentreX = 0.5 * (Nodes[i].CentreX + Nodes[j].CentreX);
            result.CentreY = 0.5 * (Nodes[i].CentreY + Nodes[j].CentreY);
            result.Radius = 0.5 * distance + Math.Max(Nodes[i].Radius, Nodes[j].Radius);

            return result;
        }


        private int GetNumberOfPoints(int minimumNumberOfPoints)
        {
            int numberOfPoints = 1;
            while(numberOfPoints < minimumNumberOfPoints && numberOfPoints < MaximumFootPointCount)
            {
                numberOfPoints *= 2;
            }
            return numberOfPoints;
        }
    }

    public struct TreeNode
    {
        public double Minparameter;
        public double MaxParameter;
        public double Radius;
        public double CentreX;
        public double CentreY;
        public int ChildNodeStartIndex;
        public int ChildNodeEndIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DistanceSquared(double x, double y)
        {
            double dx = CentreX - x;
            double dy = CentreY - y;
            return dx * dx + dy * dy;
        }
    }

    public struct NodeDistance
    {
        public int Index;
        public double Distance;
    }

}
