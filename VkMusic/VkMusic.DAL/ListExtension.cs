using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public static class ListExtension
    {
        private static readonly Random random = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            for (int i = 0; i < n; i++)
            {
                int j = random.Next(n - i);
                list.Swap(i, j);
            }

            return list;
        }

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;

            return list;
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
            => listToClone.Select(item => (T)item.Clone()).ToList();
        
    }
}
