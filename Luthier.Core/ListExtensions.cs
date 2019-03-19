using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Core
{
    public static class ListExtensions
    {
        
        public static IEnumerable<Tuple<T,T>> EnumeratePairsOpen<T>(this List<T> list)
        {
            for(int i = 0; i < list.Count - 1; i++)
            {
                yield return new Tuple<T, T>(list[i], list[i + 1]);
            }
        }

        public static IEnumerable<Tuple<T, T>> EnumeratePairsClosed<T>(this List<T> list)
        {
            if (list.Count < 2) yield break;
            for (int i = 0; i < list.Count - 1; i++)
            {
                yield return new Tuple<T, T>(list[i], list[i + 1]);
            }
            yield return new Tuple<T, T>(list[list.Count - 1], list[0]);
        }
    }
}
