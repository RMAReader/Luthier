using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Luthier.Model;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using Luthier.Model.MouseController;
using Luthier.Model.Presenter;

namespace Luthier.Test.Model
{
    [TestClass]
    public class MouseControllerInsertLinkedLineTests
    {

        [TestMethod]
        public void LeftButtonDown_LineNotInProgress()
        {
            var doc = new ApplicationDocumentModel();
            var controller = new InsertLinkedLine(doc.LinkedLine2DFactory());

            controller.MouseLeftButtonDown(1, 0);

            var line = (GraphicLinkedLine2D) doc.Model.First(x => x is GraphicLinkedLine2D);
            var p1 = (GraphicPoint2D) doc.Model.First(x => x is GraphicPoint2D);
            var p2 = (GraphicPoint2D) doc.Model.Where(x => x is GraphicPoint2D).ElementAt(1);

            Assert.AreEqual(3, doc.Model.Count());
            Assert.AreEqual(p1.Key, line.pointsKeys[0]);
            Assert.AreEqual(p2.Key, line.pointsKeys[1]);
            Assert.AreEqual(1, p1.X);
            Assert.AreEqual(1, p2.X);
            Assert.AreEqual(0, p1.Y);
            Assert.AreEqual(0, p2.Y);

            Assert.AreEqual(true, controller.lineInProgress);
        }


        [TestMethod]
        public void LeftButtonDown_LineInProgress()
        {
            var doc = new ApplicationDocumentModel();
            var controller = new InsertLinkedLine(doc.LinkedLine2DFactory());

            controller.MouseLeftButtonDown(1, 0);
            controller.MouseLeftButtonDown(2, 3);

            var line = (GraphicLinkedLine2D)doc.Model.First(x => x is GraphicLinkedLine2D);
            var p1 = (GraphicPoint2D)doc.Model.First(x => x is GraphicPoint2D);
            var p2 = (GraphicPoint2D)doc.Model.Where(x => x is GraphicPoint2D).ElementAt(1);
            var p3 = (GraphicPoint2D)doc.Model.Where(x => x is GraphicPoint2D).ElementAt(2);

            Assert.AreEqual(4, doc.Model.Count());
            Assert.AreEqual(p1.Key, line.pointsKeys[0]);
            Assert.AreEqual(p2.Key, line.pointsKeys[1]);
            Assert.AreEqual(p3.Key, line.pointsKeys[2]);

            Assert.AreEqual(1, p1.X);
            Assert.AreEqual(0, p1.Y);

            Assert.AreEqual(2, p2.X);
            Assert.AreEqual(3, p2.Y);

            Assert.AreEqual(2, p3.X);
            Assert.AreEqual(3, p3.Y);

            Assert.AreEqual(true, controller.lineInProgress);
        }


        [TestMethod]
        public void MouseMove_LineInProgress()
        {
            var doc = new ApplicationDocumentModel();
            var controller = new InsertLinkedLine(doc.LinkedLine2DFactory());

            controller.MouseLeftButtonDown(1, 0);
            controller.MouseMove(2, 3);

            var line = (GraphicLinkedLine2D)doc.Model.First(x => x is GraphicLinkedLine2D);
            var p1 = (GraphicPoint2D)doc.Model.First(x => x is GraphicPoint2D);
            var p2 = (GraphicPoint2D)doc.Model.Where(x => x is GraphicPoint2D).ElementAt(1);

            Assert.AreEqual(3, doc.Model.Count());
            Assert.AreEqual(p1.Key, line.pointsKeys[0]);
            Assert.AreEqual(p2.Key, line.pointsKeys[1]);

            Assert.AreEqual(1, p1.X);
            Assert.AreEqual(0, p1.Y);

            Assert.AreEqual(2, p2.X);
            Assert.AreEqual(3, p2.Y);

            Assert.AreEqual(true, controller.lineInProgress);
        }


        [TestMethod]
        public void RightButtonDown_LineInProgress()
        {
            var doc = new ApplicationDocumentModel();
            var controller = new InsertLinkedLine(doc.LinkedLine2DFactory());

            controller.MouseLeftButtonDown(1, 0);
            controller.MouseRightButtonDown(2, 3);

            var line = (GraphicLinkedLine2D)doc.Model.First(x => x is GraphicLinkedLine2D);
            var p1 = (GraphicPoint2D)doc.Model.First(x => x is GraphicPoint2D);
            var p2 = (GraphicPoint2D)doc.Model.Where(x => x is GraphicPoint2D).ElementAt(1);

            Assert.AreEqual(3, doc.Model.Count());
            Assert.AreEqual(p1.Key, line.pointsKeys[0]);
            Assert.AreEqual(p2.Key, line.pointsKeys[1]);

            Assert.AreEqual(1, p1.X);
            Assert.AreEqual(0, p1.Y);

            Assert.AreEqual(2, p2.X);
            Assert.AreEqual(3, p2.Y);

            Assert.AreEqual(false, controller.lineInProgress);
        }

        [TestMethod]
        public void Close_LineInProgress_ShortLine()
        {
            var doc = new ApplicationDocumentModel();
            var controller = new InsertLinkedLine(doc.LinkedLine2DFactory());

            controller.MouseLeftButtonDown(1, 0);
            controller.Close();

            Assert.AreEqual(0, doc.Model.Count());
            Assert.AreEqual(false, controller.lineInProgress);
        }


        [TestMethod]
        public void Close_LineInProgress_LongLine()
        {
            var doc = new ApplicationDocumentModel();
            var controller = doc.MouseControllerFactory().InserLinekedLine();
            
            controller.MouseLeftButtonDown(1, 0);
            controller.MouseLeftButtonDown(2, 3);
            controller.Close();

            var line = (GraphicLinkedLine2D)doc.Model.First(x => x is GraphicLinkedLine2D);
            var p1 = (GraphicPoint2D)doc.Model.First(x => x is GraphicPoint2D);
            var p2 = (GraphicPoint2D)doc.Model.Where(x => x is GraphicPoint2D).ElementAt(1);

            Assert.AreEqual(3, doc.Model.Count());
            Assert.AreEqual(p1.Key, line.pointsKeys[0]);
            Assert.AreEqual(p2.Key, line.pointsKeys[1]);

            Assert.AreEqual(1, p1.X);
            Assert.AreEqual(0, p1.Y);

            Assert.AreEqual(2, p2.X);
            Assert.AreEqual(3, p2.Y);

            Assert.AreEqual(false, controller.lineInProgress);
        }


    }




}
