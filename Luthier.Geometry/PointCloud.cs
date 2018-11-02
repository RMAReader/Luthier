using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class PointCloud
    {
        private int _dimension;
        private int _pointCount;
        private double[] _data;

        public int PointCount => _pointCount;
        public int Dimension => _dimension;

        public PointCloud(List<double[]> points)
        {
            _dimension = points.First().Length;
            _pointCount = points.Count;
            _data = new double[_dimension * _pointCount];
            for(int i=0; i < _pointCount; i++ )
            {
                Array.Copy(points[i], 0, _data, i * _dimension, _dimension);
            }
        }

        public double[] this[int index]
        {
            get
            {
                double[] result = new double[_dimension];
                Array.Copy(_data, index * _dimension, result, 0, _dimension);
                return result;
            } 
        }

        public void GetPointFast(int index, double[] result)
        {
            Array.Copy(_data, index * _dimension, result, 0, _dimension);
        }

    }
}
