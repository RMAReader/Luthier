using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Luthier.Model.CustomSettings
{
    [SettingsSerializeAsAttribute(SettingsSerializeAs.Xml)]
    public class GraphicPlaneGridAppearence
    {
        public int MajorAxisStep;
        public SharpDX.Vector4 MajorAxisColour;
        public SharpDX.Vector4 MinorAxisColour;
    }
}

