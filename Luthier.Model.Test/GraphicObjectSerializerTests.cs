using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Luthier.Model.GraphicObjects;
using Luthier.Core;
using Luthier.Model;
using System.IO;
using System.Collections.Generic;
using Luthier.CncOperation;
using Luthier.CncTool;

namespace Luthier.Test.Model
{
    [TestClass]
    public class GraphicObjectSerializerTests
    {
        [TestMethod]
        public void UniqueKeySerialization()
        {
            var key1 = new UniqueKey();
            var bytes = Serializer<UniqueKey>.Serialize(key1);

            var key2 = Serializer<UniqueKey>.Deserialize(bytes);

            Assert.AreEqual(key1, key2);
            Assert.AreNotSame(key1, key2);
        }

        [TestMethod]
        public void ModelSerialization_GraphicPoint2D()
        {
            var doc1 = new ApplicationDocumentModel();
            var p1 = doc1.Point2DFactory().New(1.0f, 2.0f);
            
            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);

       
        }

        [TestMethod]
        public void ModelSerialization_GraphicPolygon2D()
        {
            var doc1 = new ApplicationDocumentModel();

            var k4 = doc1.Polygon2DFactory().New();
            var k5 = doc1.Polygon2DFactory().AppendPoint(k4, -1, 0);
            var k6 = doc1.Polygon2DFactory().AppendPoint(k4, 0, 0);
            var k7 = doc1.Polygon2DFactory().AppendPoint(k4, 0, 1);

            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }

        [TestMethod]
        public void ModelSerialization_GraphicLinkedLine2D()
        {
            var doc1 = new ApplicationDocumentModel();

            var k4 = doc1.LinkedLine2DFactory().New();
            var k5 = doc1.LinkedLine2DFactory().AppendPoint(k4, -1, 0);
            var k6 = doc1.LinkedLine2DFactory().AppendPoint(k4, 0, 0);
            var k7 = doc1.LinkedLine2DFactory().AppendPoint(k4, 0, 1);

            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }

        [TestMethod]
        public void ModelSerialization_GraphicLengthGauge()
        {
            var doc1 = new ApplicationDocumentModel();

            var k4 = doc1.LengthGaugeFactory().New(1.0f, 2.0f);
            doc1.LengthGaugeFactory().SetEndPoint(k4, 5.0f, 2.0f);
            
            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }

        [TestMethod]
        public void ModelSerialization_GraphicImage()
        {
            var doc1 = new ApplicationDocumentModel();

            var k4 = doc1.ImageFactory().New(1.0f, 2.0f);
            doc1.ImageFactory().SetPointsFixedAspectRatio(k4, 1.0f, 2.0f, 9.0f, 1.0f);

            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }

        [TestMethod]
        public void ModelSerialization_GraphicBSplineCurve()
        {
            var doc1 = new ApplicationDocumentModel();

            var k4 = doc1.BSplineFactory().New(2);
            doc1.BSplineFactory().AppendPoint(k4, 0, 0);
            doc1.BSplineFactory().AppendPoint(k4, 2, 0);
            doc1.BSplineFactory().AppendPoint(k4, 3, 5);
            
            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }

        [TestMethod]
        public void ModelSerialization_GraphicIntersection()
        {
            var doc1 = new ApplicationDocumentModel();

            var k1 = doc1.IntersectionFactory().New(0,0);
            doc1.IntersectionFactory().SetRadius(k1, 20, 20);
            doc1.IntersectionFactory().SetObject1(k1, doc1.BSplineFactory().New(2));
            doc1.IntersectionFactory().SetObject2(k1, doc1.BSplineFactory().New(2));

            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }


        [TestMethod]
        public void ModelSerialization_GraphicCompositePolygon()
        {
            var doc1 = new ApplicationDocumentModel();

            var p1 = doc1.CompositePolygonFactory().New();
            var i1 = doc1.IntersectionFactory().New(0, 0);

            doc1.CompositePolygonFactory().AddIntersection(p1, i1);

            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }


        [TestMethod]
        public void ModelSerialization_GraphicNurbsSurface()
        {
            var surface = new GraphicNurbSurface(2, false, 3, 3, 3, 3);
            surface.cvArray = new double[36];
            for(int i = 0; i < 36; i++) surface.cvArray[i] = i;
            surface.knotArray0 = new double[] {0,1,2,3};
            surface.knotArray1 = new double[] {0,1,2,3};


            var bytes = Serializer<GraphicNurbSurface>.Serialize(surface);

            var surface2 = Serializer<GraphicNurbSurface>.Deserialize(bytes);
            Assert.AreEqual(surface.Key, surface2.Key);
            Assert.AreEqual(surface.order0, surface2.order0);

            var model1 = new GraphicModel();
            model1.Objects.Add(surface);

            bytes = Serializer<GraphicModel>.Serialize(model1);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);

            Assert.AreEqual(model1.Objects[0].Key, model2.Objects[0].Key);
        }

        [TestMethod]
        public void ModelSerialization_PocketSpecification()
        {
            var doc1 = new ApplicationDocumentModel();

            var polygon = doc1.Polygon2DFactory().New();
            var p1 = doc1.Polygon2DFactory().AppendPoint(polygon, -1, 0);
            var p2 = doc1.Polygon2DFactory().AppendPoint(polygon, 0, 0);
            var p3 = doc1.Polygon2DFactory().AppendPoint(polygon, 0, 1);

            var factory = doc1.ToolPathFactory().PocketFactory();

            var spec = factory.New();
            factory.AddBoundaryPolygons(spec, new List<GraphicPolygon2D> { polygon });
            factory.SetTool(spec, new EndMill());
            spec.CutHeight = 100;
            spec.Description = "Description text";
            spec.FeedRate = 500;
            spec.Name = "PocketSpec";
            spec.SafeHeight = 150;
            spec.SpindleSpeed = 24000;
            spec.SpindleState = EnumSpindleState.OnClockwise;
            spec.StepLength = 2;
            

            var bytes = Serializer<GraphicModel>.Serialize(doc1.model);

            var model2 = Serializer<GraphicModel>.Deserialize(bytes);
        }

    }
}
