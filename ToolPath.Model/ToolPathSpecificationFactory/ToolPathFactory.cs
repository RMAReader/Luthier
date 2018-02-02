using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathSpecificationFactory
{
    public class ToolPathFactory : IToolPathFactory
    {
        private ApplicationDocumentModel model;

        public ToolPathFactory(ApplicationDocumentModel model)
        {
            this.model = model;
        }

        public ICurveSpecificationFactory CurveFactory() => new CurveSpecificationFactory(model);
        

        public IPocketSpecificationFactory PocketFactory() => new PocketSpecificationFactory(model);


    }
}
