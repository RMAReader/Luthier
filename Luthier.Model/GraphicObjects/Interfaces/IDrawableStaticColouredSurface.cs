﻿using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects.Interfaces
{
    interface IDrawableStaticColouredSurface
    {
        void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices);
    }
}
