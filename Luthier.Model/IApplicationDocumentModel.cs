﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using Luthier.Model.MouseController;
using Luthier.Model.ToolPathSpecificationFactory;

namespace Luthier.Model
{
    public interface IApplicationDocumentModel
    {
        Dictionary<UniqueKey, GraphicObjectBase> Objects();
        IPoint2DFactory Point2DFactory();
        IPolygon2DFactory Polygon2DFactory();
        ILinkedLine2DFactory LinkedLine2DFactory();
        BSplineFactory BSplineFactory();
        GraphicImageFactory ImageFactory();
        LengthGaugeFactory LengthGaugeFactory();
        IntersectionFactory IntersectionFactory();
        CompositePolygonFactory CompositePolygonFactory();

        IAdapterSystemDrawing AdapterSystemDrawing();
        IMouseControllerFactory MouseControllerFactory();
        IToolPathFactory ToolPathFactory();

        byte[] SerialiseToBytes();
        void DeserialiseFromBytes(byte[] bytes);
        void New();
    }
}