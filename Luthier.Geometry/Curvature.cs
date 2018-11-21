using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class Curvature
    {


        /// <summary>
        /// returns a vector from point on parametric curve C(t) to the centre of its osculating circle
        /// </summary>
        /// <param name="d1">first derivative w.r.t. curve parameter</param>
        /// <param name="d2">second derivative w.r.t. curve parameter</param>
        /// <param name="result"></param>
        public static void CentreOfCurvature_TwoDimensions(double[] d1, double[] d2, ref double[] result)
        {
            double a = (d1[0] * d1[0] + d1[1] * d1[1]) / (d1[0] * d2[1] - d2[0] * d1[1]);

            result[0] = -d1[1] * a;
            result[1] = d1[0] * a;
        }

        /// <summary>
        /// returns a vector from point on parametric curve C(t) to the centre of its osculating circle
        /// </summary>
        /// <param name="d1">first derivative w.r.t. curve parameter</param>
        /// <param name="d2">second derivative w.r.t. curve parameter</param>
        /// <param name="result"></param>
        public static void CentreOfCurvature_ThreeDimensions(double[] d1, double[] d2, ref double[] result)
        {
            
            //components in direction of Binormal: B = d1 X d2
            double zy = d2[2] * d1[1] - d2[1] * d1[2];
            double xz = d2[0] * d1[2] - d2[2] * d1[0];
            double yx = d2[1] * d1[0] - d2[0] * d1[1];

            //components in direction of Normal: N = B x d1
            result[0] = xz * d1[2] - yx * d1[1];
            result[1] = yx * d1[0] - zy * d1[2];
            result[2] = zy * d1[1] - xz * d1[0];

            double a = (d1[0] * d1[0] + d1[1] * d1[1] + d1[2] * d1[2]);
            double b = zy * zy + xz * xz + yx * yx;
            double c = result[0] * result[0] + result[1] * result[1] + result[2] * result[2];

            a *= Math.Sqrt(a / b / c);

            result[0] *= a;
            result[1] *= a;
            result[2] *= a;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1">first derivative w.r.t. curve parameter</param>
        /// <param name="d2">second derivative w.r.t. curve parameter</param>
        /// <param name="tangent">unit tangent vector</param>
        /// <param name="normal">unit normal vector</param>
        /// <param name="binormal">unit binormal vector</param>
        /// <param name="curvature">signed curvature</param>
        public static void Curvature_TwoDimensions(double[] d1, double[] d2, ref double[] tangent, ref double[] normal, ref double curvature)
        {
            curvature = 1 / (d1[0] * d1[0] + d1[1] * d1[1]);
            double a = Math.Sqrt(curvature);

            curvature *= a * (d1[0] * d2[1] - d2[0] * d1[1]);

            tangent[0] = d1[0] * a;
            tangent[1] = d1[1] * a;

            normal[0] = -tangent[1];
            normal[1] = tangent[0];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1">first derivative w.r.t. curve parameter</param>
        /// <param name="d2">second derivative w.r.t. curve parameter</param>
        /// <param name="tangent">unit tangent vector</param>
        /// <param name="normal">unit normal vector</param>
        /// <param name="binormal">unit binormal vector</param>
        /// <param name="curvature">signed curvature</param>
        public static void Curvature_ThreeDimensions(double[] d1, double[] d2, ref double[] tangent, ref double[] normal, ref double[] binormal, ref double curvature)
        {
            //components in direction of Binormal: B = d1 X d2
            binormal[0] = d2[2] * d1[1] - d2[1] * d1[2];
            binormal[1] = d2[0] * d1[2] - d2[2] * d1[0];
            binormal[2] = d2[1] * d1[0] - d2[0] * d1[1];

            //components in direction of Tangent
            double a = d1[0] * d1[0] + d1[1] * d1[1] + d1[2] * d1[2];
            double b = 1 / Math.Sqrt(a);
            double c = 1 / Math.Sqrt(binormal[0] * binormal[0] + binormal[1] * binormal[1] + binormal[2] * binormal[2]);
            
            curvature =  b / (a * c);

            tangent[0] = d1[0] * b;
            tangent[1] = d1[1] * b;
            tangent[2] = d1[2] * b;

            binormal[0] *= c;
            binormal[1] *= c;
            binormal[2] *= c;

            normal[0] = binormal[1] * tangent[2] - binormal[2] * tangent[1];
            normal[1] = binormal[2] * tangent[0] - binormal[0] * tangent[2];
            normal[2] = binormal[0] * tangent[1] - binormal[1] * tangent[0];
        }

      
    }
}
