using Luthier.CncOperation;
using Luthier.CncTool;
using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathCalculator;
using System;
using System.Xml.Serialization;

namespace Luthier.Model.ToolPathSpecification
{
    [Serializable]
    public class ToolPathSpecificationBase : GraphicObjectBase
    {
       
        [XmlIgnore]
        public ToolPath.ToolPath ToolPath { get; set; }


        public virtual ToolPathCalculatorBase GetCalculator(IApplicationDocumentModel model) => new ToolPathCalculatorBase();

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return float.MaxValue;
        }

        public override string ToString() => "";
       
    }
}
