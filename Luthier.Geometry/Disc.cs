using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Geometry
{
    [Serializable]
    public class Disc
    {
        [XmlElement]
        public Point3D Centre;
        [XmlElement]
        public Point3D Normal;
        [XmlElement]
        public double Radius;


        public List<Point3D> PerimeterToLines(int n)
        {
            var result = new List<Point3D>();

            //create orthonormal basis
            Point3D u, v;
            if(Math.Abs(Normal.X) <= Math.Abs(Normal.Y) && Math.Abs(Normal.X) <= Math.Abs(Normal.Z))
            {
                u = new Point3D(0, -Normal.Z, Normal.Y).Normalise();
            }
            else if (Math.Abs(Normal.Y) <= Math.Abs(Normal.X) && Math.Abs(Normal.Y) <= Math.Abs(Normal.Z))
            {
                u = new Point3D(-Normal.Z, 0, Normal.X).Normalise();
            }
            else
            {
                u = new Point3D(-Normal.Y, Normal.X, 0).Normalise();
            }

            v = u.VectorProduct(Normal).Normalise();

            //create perimeter points
            for (int i=0; i<n; i++)
            {
                double angle = Math.PI * 2 * i / n;
                var point = Centre + Math.Cos(angle) * Radius * u + Math.Sin(angle) * Radius * v;
                result.Add(point);
            }

            return result;
        }


        public Disc DeepCopy()
        {
            return new Disc
            {
                Centre = new Point3D(Centre.X, Centre.Y, Centre.Z),
                Normal = new Point3D(Normal.X, Normal.Y, Normal.Z),
                Radius = Radius
            };
        }

     
    }
}
