/* © 2020 - Greg Waller.  All rights reserved. */

using System.Collections.Generic;

namespace LRG
{
    using Random = System.Random;

    public static class Utility
    {
        private readonly static Random _rng = new Random();

        public static void Shuffle<T>(this List<T> list, int seed = 0)
        {
            Random rng = seed == 0 ? _rng : new Random(seed);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        public static T Random<T>(this List<T> list, int seed = 0)
        {
            Random rng = seed == 0 ? _rng : new Random(seed);
            return list[rng.Next(list.Count)];
        }
    }
}