using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    public interface ICncOperation
    {
        string accept(ICncOperationLanguageVisitor visitor);
    }
}
