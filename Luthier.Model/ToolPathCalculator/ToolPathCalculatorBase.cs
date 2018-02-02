using Luthier.CncOperation;
using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathCalculator
{
    public class ToolPathCalculatorBase
    {
        public virtual ToolPath.ToolPath Execute() => new ToolPath.ToolPath(); 



        protected ToolPath.ToolPath path;
        //public ToolPath.ToolPath ToolPath { get => path; }
    }
}
