using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class Knot
    {
        public int p;
        public List<double> data;

        public double minParam { get => data[p]; }
        public double maxParam { get => data[data.Count-1-p]; }
        public int size { get => data.Count; }

        public static int MinIndex(double[] knot, int order) => order - 1;
        public static int MaxIndex(double[] knot, int order) => knot.Length - order;


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
            output.Normalise();
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
        public void Normalise() => data = data.Select(x => x = (x - minParam) / (maxParam - minParam)).ToList();
        
        public static double[] Reverse(double[] knot)
        {
            return knot.Reverse().ToArray();
        }


        public Knot DeepCopy()
        {
            return new Knot {p = p, data = new List<double>(data) };
        }

        public static double[] Insert(double[] knot, params double[] newKnots)
        {
            var result = new List<double>(knot);
            result.AddRange(newKnots);
            result.Sort();
            return result.ToArray();
        }
    }
}
