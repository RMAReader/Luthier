using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathSpecification
{
 
    public abstract class BaseCommand : GraphicObjectBase
    {
        private IApplicationDocumentModel model;

        public BaseCommand(IApplicationDocumentModel model)
        {
            this.model = model;
        }

        public virtual void execute() { }
    }
}
