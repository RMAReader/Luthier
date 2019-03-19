using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class Plane3D
    {
        public Point3D U;
        public Point3D V;
        public Point3D Normal;
        public Point3D Origin;


        public double[][] GetMapGlobalToPlaneCoordinates()
        {
            return new double[][] { U.Data, V.Data, Normal.Data };
        }
       
    }
}
