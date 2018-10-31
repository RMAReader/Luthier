using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.CustomSettings
{
    [SettingsSerializeAsAttribute(SettingsSerializeAs.Xml)]
    public class GraphicNurbsCurveAppearance
    {
        public SharpDX.Vector4 CurveColour;
        public SharpDX.Vector4 ControlColour;
    }
}
