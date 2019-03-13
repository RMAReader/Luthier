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
    [XmlInclude(typeof(GraphicImage2d))]
    [XmlInclude(typeof(GraphicBSplineCurve))]
    [XmlInclude(typeof(GraphicLengthGauge))]
    [XmlInclude(typeof(GraphicIntersection))]
    [XmlInclude(typeof(GraphicCompositePolygon))]
    [XmlInclude(typeof(GraphicNurbsCurve))]
    [XmlInclude(typeof(GraphicNurbsSurface))]
    [XmlInclude(typeof(GraphicLayer))]
    [XmlInclude(typeof(GraphicPlane))]
    [XmlInclude(typeof(GraphicImage3d))]
    [XmlInclude(typeof(PocketSpecification))]
    [XmlInclude(typeof(EndMill))]
    [XmlInclude(typeof(BallNose))]
    public class GraphicModelStorage
    {
        [XmlArray()]
        public List<GraphicObjectBase> Objects { get; set; }

        [XmlElement()]
        public string Name { get; set; }

        public GraphicModelStorage()
        {
            Objects = new List<GraphicObjectBase>();
        }
    }


    public class GraphicModel : IEnumerable<GraphicObjectBase>
    {
        
        private readonly Dictionary<UniqueKey, GraphicObjectBase> _data;

        public event EventHandler<ModelChangeEventArgs> ModelChangedHandler;

        public string Path { get; set; }

        public string Name { get; set; }

        public bool HasChanged { get; set; }

        public GraphicModelStorage GetStorage() => new GraphicModelStorage { Objects = _data.Values.ToList(), Name = this.Name };

        public GraphicModel()
        {
            _data = new Dictionary<UniqueKey, GraphicObjectBase>();
            HasChanged = true;
            Name = "NewModel";
        }
        public GraphicModel(GraphicModelStorage storage)
        {
            _data = storage.Objects.ToDictionary(x => x.Key);
            foreach (var obj in _data.Values) BindGraphicObject(obj); 
            HasChanged = true;
            Name = storage.Name;
        }
        public GraphicModel(GraphicModelStorage storage, string path)
        {
            _data = storage.Objects.ToDictionary(x => x.Key);
            foreach (var obj in _data.Values) BindGraphicObject(obj);

            HasChanged = true;
            Name = storage.Name ?? path;
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


        public void Add(GraphicObjectBase obj, bool raiseChangeEvent = true)
        {
            SetDefaultName(obj);
            BindGraphicObject(obj);
            _data.Add(obj.Key, obj);

            if (raiseChangeEvent)
            {
                var e = new ModelChangeEventArgs { ObjectAdded = obj };
                OnModelChanged(e);
            }
        }

        public void Remove(GraphicObjectBase obj)
        {
            _data.Remove(obj.Key);
            UnbindGraphicObject(obj);
        }

        public IEnumerable<IDraggable2d> GetDraggableObjects2d()
        {
            foreach (IDraggable2d o in _data.Values.Where(p => p is IDraggable2d)) yield return o;
            foreach (GraphicNurbsSurface s in _data.Values.Where(p => p is GraphicNurbsSurface))
            {
                foreach (IDraggable2d o in s.GetDraggableObjects2d()) yield return o;
            }
            foreach (GraphicNurbsCurve c in _data.Values.Where(p => p is GraphicNurbsCurve))
            {
                foreach (IDraggable2d o in c.GetDraggableObjects()) yield return o;
            }
        }
        public IEnumerable<IDraggable> GetDraggableObjects()
        {
            foreach (IDraggable o in _data.Values.Where(p => p is IDraggable)) yield return o;
            foreach (GraphicNurbsSurface s in _data.Values.Where(p => p is GraphicNurbsSurface))
            {
                foreach (IDraggable o in s.GetDraggableObjects()) yield return o;
            }
            foreach (GraphicNurbsCurve c in _data.Values.Where(p => p is GraphicNurbsCurve))
            {
                foreach (IDraggable o in c.GetDraggableObjects()) yield return o;
            }
        }
        public IEnumerable<GraphicObjectBase> VisibleObjects()
        {
            return _data.Values.Where(p => p.IsVisible);
        }


        public void RaiseModelChangedEvent()
        {
            OnModelChanged(new ModelChangeEventArgs());
        }

        private void OnModelChanged(ModelChangeEventArgs e)
        {
            HasChanged = true;
            ModelChangedHandler?.Invoke(this, e);
        }

        private void SetDefaultName(GraphicObjectBase obj)
        {
            if (String.IsNullOrEmpty(obj.Name))
            {
                var prefix = obj.GetType().Name;

                var count = 0;
                foreach (var x in _data.Values.Where(x => x.GetType() == obj.GetType() && x.Name.Substring(0, Math.Min(prefix.Length,x.Name.Length)) == prefix))
                {
                    if (Int32.TryParse(x.Name.Substring(prefix.Length, x.Name.Length - prefix.Length), out int suffix))
                    {
                        if(count < suffix) count = suffix;
                    }
                }
                obj.Name = $"{prefix}{count + 1}";
            }
        }


        private void BindGraphicObject(GraphicObjectBase obj)
        {
            obj.Model = this;
        }

        private void UnbindGraphicObject(GraphicObjectBase obj)
        {
            obj.Model = null;
        }
    }

}
