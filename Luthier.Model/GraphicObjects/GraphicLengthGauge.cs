using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicLengthGauge : GraphicObjectBase
    {
        [XmlElement]
        public UniqueKey fromPoint;
        [XmlElement]
        public UniqueKey toPoint;

        public GraphicLengthGauge() { }
        public GraphicLengthGauge(UniqueKey from, UniqueKey to) { fromPoint = from; toPoint = to; }


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return float.MaxValue;
        }
    }
}
