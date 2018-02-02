using Luthier.CncTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.ToolPath
{
    public class CncToolRepository : ICncToolRepository
    {
        private List<BaseTool> data = new List<BaseTool>
        {
            new BallNose{ Name = "Ball Nose 6.35mm", Diameter = 6.35f },
            new EndMill{ Name = "End Mill 6.35mm", Diameter = 6.35f }
        };

        public List<BaseTool> GetAllTools() => data;
    }
}
