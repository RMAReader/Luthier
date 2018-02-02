using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.BSpline
{
    public class Knot
    {
        public int p;
        public List<double> data;

        public double minParam { get => data[p]; }
        public double maxParam { get => data[data.Count-1-p]; }
        public int size { get => data.Count; }
        
        public static Knot CreateUniformOpen(int p, int n)
        {
            Knot output = new Knot();
            output.p = p;
            output.data = new List<double>();
            for (int i = 0; i < n; i++)
            {
                output.data.Add(i);
            }
            output.Normalise();
            return output;
        }

        public static Knot CreateUniformClosed(int p, int n)
        {
            Knot output = CreateUniformOpen(p, n);
            output.CloseFront();
            output.CloseBack();
            return output;
        }

        public void CloseFront()
        {
            for (int i = 0; i <= p; i++) data[i] = minParam;
        }

        public void CloseBack()
        {
            for (int i = data.Count() - p - 1; i < data.Count(); i++) data[i] = maxParam;
        }

        public void insert(List<double> k)
        {
            data.AddRange(k);
            data.Sort();
        }

        public void AppendOpen()
        {
            data = CreateUniformOpen(p, data.Count() + 1).data;
        }

        public void push_back_closed()
        {
            data = CreateUniformClosed(p, data.Count() + 1).data;
        }

        //returns continuity of curve at parameter x
        public int Continuity(double x) => p + 1 - data.Where(y => y == x).Count();
           
        //returns multiplicity of knot i
        public int Multiplicity(int knotIX) => data.Where(x => x == data[knotIX]).Count();
        
        //rescales knot vector so parameter is in range [0, 1]
        public void Normalise() => data.ForEach(x => x = (x - minParam) / (maxParam - minParam));
        

        public Knot DeepCopy()
        {
            return new Knot {p = p, data = new List<double>(data) };
        }
    }
}
