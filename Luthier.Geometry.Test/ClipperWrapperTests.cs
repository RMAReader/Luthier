using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class ClipperWrapperTests
    {

        [TestMethod]
        public void Test1_offsetPolygon()
        {
            var polygon = new Polygon2D(new List<Point2D>
            {
                new Point2D(-10, 0),
                new Point2D(10, 0),
                new Point2D(10, 10),
                new Point2D(0, 20),
                new Point2D(0, 30)
            });

            var expected = new List<Point2D>
            {
                new Point2D(-10, 1),
                new Point2D(9, 1),
                new Point2D(9, 9.585786437626906),
                new Point2D(-1,19.5857864376269),
                new Point2D(-1,30)
            };

            var actual = ClipperWrapper.OffsetPolygon(polygon, -1);
            
        }
    }
}
