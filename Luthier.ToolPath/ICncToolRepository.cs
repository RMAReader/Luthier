using Luthier.CncTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.ToolPath
{
    public interface ICncToolRepository
    {
        List<BaseTool> GetAllTools();
    }
}
