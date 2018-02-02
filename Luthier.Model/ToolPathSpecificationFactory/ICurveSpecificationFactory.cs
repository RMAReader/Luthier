using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathSpecificationFactory
{
    public interface ICurveSpecificationFactory
    {
        CurveSpecification New();
    }
}
