using Luthier.CncTool;
using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    [XmlInclude(typeof(GraphicPoint2D))]
    [XmlInclude(typeof(GraphicLinkedLine2D))]
    [XmlInclude(typeof(GraphicPolygon2D))]
    [XmlInclude(typeof(GraphicImage))]
    [XmlInclude(typeof(GraphicBSplineCurve))]
    [XmlInclude(typeof(GraphicLengthGauge))]
    [XmlInclude(typeof(GraphicIntersection))]
    [XmlInclude(typeof(GraphicCompositePolygon))]
    [XmlInclude(typeof(GraphicNurbSurface))]
    [XmlInclude(typeof(PocketSpecification))]
    [XmlInclude(typeof(EndMill))]
    [XmlInclude(typeof(BallNose))]
    public class GraphicModelStorage
    {
        [XmlArray()]
        public List<GraphicObjectBase> Objects { get; set; }

        public GraphicModelStorage()
        {
            Objects = new List<GraphicObjectBase>();
        }
    }


    public class GraphicModel : IEnumerable<GraphicObjectBase>
    {
        private readonly Dictionary<UniqueKey, GraphicObjectBase> _data;

        public bool HasChanged { get; set; }

        public GraphicModelStorage GetStorage => new GraphicModelStorage { Objects = _data.Values.ToList() };

        public GraphicModel()
        {
            _data = new Dictionary<UniqueKey, GraphicObjectBase>();
            HasChanged = true;
        }
        public GraphicModel(GraphicModelStorage storage)
        {
            _data = storage.Objects.ToDictionary(x => x.Key);
            HasChanged = true;
        }

        public GraphicObjectBase this[UniqueKey key] => _data[key];

        public bool ContainsKey(UniqueKey key) => _data.ContainsKey(key);
        public bool ContainsObject(GraphicObjectBase obj) => _data.ContainsKey(obj.Key);

        public IEnumerator<GraphicObjectBase> GetEnumerator()
        {
            return _data.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.Values.GetEnumerator();
        }


        public void Add(GraphicObjectBase obj)
        {
            _data.Add(obj.Key, obj);
            HasChanged = true;
        }

        public void Remove(GraphicObjectBase obj)
        {
            _data.Remove(obj.Key);
            HasChanged = true;
        }

        public IEnumerable<IDraggable2d> GetDraggableObjects2d()
        {
            foreach (IDraggable2d o in _data.Values.Where(p => p is IDraggable2d)) yield return o;
            foreach (GraphicNurbSurface s in _data.Values.Where(p => p is GraphicNurbSurface))
            {
                foreach (IDraggable2d o in s.GetDraggableObjects2d()) yield return o;
            }
        }
        public IEnumerable<IDraggable> GetDraggableObjects()
        {
            foreach (IDraggable o in _data.Values.Where(p => p is IDraggable)) yield return o;
            foreach (GraphicNurbSurface s in _data.Values.Where(p => p is GraphicNurbSurface))
            {
                foreach (IDraggable o in s.GetDraggableObjects()) yield return o;
            }
        }
    }

}
