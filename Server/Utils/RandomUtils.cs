using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils
{
    public static class RandomUtils
    {
        [ThreadStatic] private static Random random = new Random();

        /// <summary>
        /// Uses the Fisher-Yates shuffle algorithm.
        /// See: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        /// </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 1; i--)
            {
                int j = random.Next(i + 1);

                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[random.Next(list.Count)];
        }

        public static T GetRandomElementWithPossibilities<T>(this List<T> list, List<int> possibilities)
        {
            if (possibilities.Sum() != 100)
                throw new ArgumentException("Possibilities must add up to 100.");

            int possibility = 0;
            int roll = random.Next(100);

            for (int i = 0; i < list.Count; i++)
            {
                possibility += possibilities[i];

                if (roll < possibility)
                    return list[i];
            }

            return list.Last();
        }
    }
}
