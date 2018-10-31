using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
        RayIntersection GetRayIntersection(double[] from, double[] to);
    }

    public class RayIntersection
    {
        public GraphicObjectBase Object { get; set; }
        public double[] IntersectInWorldCoords { get; set; }
        public double RayParameter { get; set; }
        public double[] ObjectParameters { get; set; }
        public bool ObjectHit { get; set; }
    }

}
