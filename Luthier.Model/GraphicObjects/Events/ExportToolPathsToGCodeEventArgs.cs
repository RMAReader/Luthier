using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections.Generic;


namespace Luthier.Model.GraphicObjects.Events
{
    public class ExportToolPathsToGCodeEventArgs : EventArgs
    {
        public List<ToolPathSpecificationBase> ToolPaths { get; set; }
    }
}
