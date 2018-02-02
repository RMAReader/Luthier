using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathSpecificationFactory
{
    public interface IToolPathFactory
    {
        IPocketSpecificationFactory PocketFactory();
        ICurveSpecificationFactory CurveFactory();
    }
}
