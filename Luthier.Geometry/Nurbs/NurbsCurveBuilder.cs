using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class NurbsCurveBuilder
    {

        public static NurbsCurve GetOffsetCurveInPlane(NurbsCurve curve, double[] offsetNormal, double offsetDistance)
        {
            //1. Get control points
            var cvs = new List<double[]>();
            for(int i=0; i< curve.ControlPoints.CvCount[0]; i++)
            {
                cvs.Add(curve.GetCV(i));
            }

            var cvsTyped = new List<Point3D>();
            for (int i = 0; i < curve.ControlPoints.CvCount[0]; i++)
            {
                cvsTyped.Add(curve.GetCV<Point3D>(i));
            }

            //2. create offset control points
            var result = curve.DeepCopy();

            var cvsOffset = Luthier.Geometry.Algorithm.offset_path(cvs, offsetDistance, false, open: true);
            
            for (int i=0; i < curve.ControlPoints.CvCount[0]; i++)
            {
                double[] newCv = curve.GetCV(i);
                newCv[0] = cvsOffset[i][0];
                newCv[1] = cvsOffset[i][1];
                
                result.SetCV(i, newCv);
            }

            return result;
        }



    }
}
