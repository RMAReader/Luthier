using Luthier.CncOperation;
using Luthier.CncTool;
using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathCalculator;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Luthier.Model.ToolPathSpecification
{
    [Serializable]
    public class ToolPathSpecificationBase : 
        GraphicObjectBase,
        IDrawableLines
    {

        public UniqueKey ReferencePlaneKey { get; set; }

        public bool IsCutFromUnderneath { get; set; }

        public GraphicPlane GetReferencePlane()
        {
                    if (IsCutFromUnderneath)
                    {
                        return new GraphicPlane
                        {
                            _origin = new double[] { 0, 0, 0 },
                            _normal = new double[] { 0, 0, -1 },
                            _unitU = new double[] { 0, 1, 0 },
                            _unitV = new double[] { -1, 0, 0 },
                        };
                    }
                    else
                    {
                        return new GraphicPlane
                        {
                            _origin = new double[] { 0, 0, 0 },
                            _normal = new double[] { 0, 0, 1 },
                            _unitU = new double[] { 0, -1, 0 },
                            _unitV = new double[] { -1, 0, 0 },
                        };
                    }
        }


        [XmlIgnore] public ToolPath.ToolPath ToolPath { get; set; }


        public virtual ToolPathCalculatorBase GetCalculator(IApplicationDocumentModel model) => new ToolPathCalculatorBase();

        public virtual void Calculate() { }


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return float.MaxValue;
        }


        #region "IDrawableLines implementation"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            if(IsVisible && ToolPath != null)
            {
                var pathColour = new SharpDX.Vector4(1, 0, 0, 1);
                var referencePlane = GetReferencePlane();

                double? x = 0;
                double? y = 0;
                double? z = 0;

                int startIndex = vertices.Count;

                foreach (var operation in ToolPath)
                {
                    var motion = operation as MoveToPoint;
                    if(motion != null)
                    {
                        if (motion.GetX() != null) x = motion.GetX();
                        if (motion.GetY() != null) y = motion.GetY();
                        if (motion.GetZ() != null) z = motion.GetZ();

                        if( x.HasValue && y.HasValue && z.HasValue )
                        {
                            var p = referencePlane.MapPlaneToWorldCoordinates(new Geometry.Point3D(x.Value, y.Value, z.Value));

                            vertices.Add(new StaticColouredVertex
                            {
                                Position = new SharpDX.Vector3((float)p.X, (float)p.Y, (float)p.Z),
                                Color = pathColour
                            });
                        }
                    }
                }

                for (int i = startIndex; i < vertices.Count - 1; i++)
                {
                    indices.AddRange(new int[] { i, i + 1 });
                }
            }
        }

        #endregion
    }
}
