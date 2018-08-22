using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathSpecificationFactory
{
    public class CurveSpecificationFactory : ICurveSpecificationFactory
    {

        private ApplicationDocumentModel model;

        public CurveSpecificationFactory(ApplicationDocumentModel model)
        {
            this.model = model;
        }


        public CurveSpecification New()
        {
            var spec = new CurveSpecification();
            model.Model.Add(spec);
            return spec;
        }
    }
}
