using Luthier.CncTool;
using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    [XmlInclude(typeof(GraphicPoint2D))]
    [XmlInclude(typeof(GraphicLinkedLine2D))]
    [XmlInclude(typeof(GraphicPolygon2D))]
    [XmlInclude(typeof(GraphicImage))]
    [XmlInclude(typeof(GraphicBSplineCurve))]
    [XmlInclude(typeof(GraphicLengthGauge))]
    [XmlInclude(typeof(GraphicIntersection))]
    [XmlInclude(typeof(GraphicCompositePolygon))]
    [XmlInclude(typeof(GraphicNurbSurface))]
    [XmlInclude(typeof(PocketSpecification))]
    [XmlInclude(typeof(EndMill))]
    [XmlInclude(typeof(BallNose))]
    public class GraphicModel
    {
        [XmlArray()]
        public List<GraphicObjectBase> Objects { get; set; }

        public GraphicModel() => Objects = new List<GraphicObjectBase>();
    }
}
