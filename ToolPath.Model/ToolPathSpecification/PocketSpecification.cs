using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Luthier.CncTool;
using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathCalculator;
using Luthier.CncOperation;

namespace Luthier.Model.ToolPathSpecification
{
    [Serializable]
    public class PocketSpecification : ToolPathSpecificationBase
    {
        public List<UniqueKey> BoundaryPolygonKey { get; set; }
        public double CutHeight { get; set; }
        public double StepLength { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double SafeHeight { get; set; }
        public EnumSpindleState SpindleState { get; set; }
        public int SpindleSpeed { get; set; }
        public int FeedRate { get; set; }
        public BaseTool Tool { get; set; }

        public PocketSpecification() 
        {
            Name = Key.data;
            BoundaryPolygonKey = new List<UniqueKey>();
        }

        public override ToolPathCalculatorBase GetCalculator(IApplicationDocumentModel model)
        {
            return new PocketCalculator(this, model);
        }


    }
}
