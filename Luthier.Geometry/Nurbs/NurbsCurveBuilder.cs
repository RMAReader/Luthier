using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class NurbsCurveBuilder
    {

        public static NurbsCurve GetOffsetCurveInPlane(NurbsCurve curve, double[] planeU, double[] planeV, double[] planeNormal, double offsetDistance)
        {

            //0. map curve to in-plane coordinates
            double[][] map = new double[][] { planeU, planeV, planeNormal };
            var inPlaneCurve = curve.Apply((double[] cv)=> map.DotProduct(cv));

            //1. Get control points
            var cvs = new List<double[]>();
            for(int i=0; i< inPlaneCurve.ControlPoints.CvCount[0]; i++)
            {
                cvs.Add(inPlaneCurve.GetCV(i));
            }

            var cvsTyped = new List<Point3D>();
            for (int i = 0; i < inPlaneCurve.ControlPoints.CvCount[0]; i++)
            {
                cvsTyped.Add(inPlaneCurve.GetCV<Point3D>(i));
            }

            //2. create offset control points
            var result = curve.DeepCopy();

            var cvsOffset = Luthier.Geometry.Algorithm.offset_path(cvs, offsetDistance, false, open: true);
            
            for (int i=0; i < inPlaneCurve.ControlPoints.CvCount[0]; i++)
            {
                double[] newCv = inPlaneCurve.GetCV(i);
                newCv[0] = cvsOffset[i][0];
                newCv[1] = cvsOffset[i][1];
                
                result.SetCV(i, newCv);
            }

            //3. transform result back into global coordinates
            double[][] reverseMap = map.Inverse();
            result = result.Apply((double[] cv) => reverseMap.DotProduct(cv));

            return result;
        }



    }
}
