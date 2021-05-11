using System.Collections.Generic;

namespace trollschmiede.CivIdle.Generic
{
    public static class Shuffle
    {
        private static System.Random rng = new System.Random();
        public static void ShuffleList<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}