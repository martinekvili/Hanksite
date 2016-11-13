using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils
{
    public static class RandomUtils
    {
        [ThreadStatic] private static Random _random;

        private static Random random
        {
            get
            {
                if (_random == null)
                    _random = new Random();

                return _random;
            }
        }

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

        public static int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        public static int NextExcluding(int maxValue, int excluded)
        {
            if (excluded >= maxValue)
                throw new ArgumentException("Excluded number is out of the boundaries");

            int randomNum = random.Next(maxValue - 1);
            return randomNum == excluded ? maxValue - 1 : randomNum;
        }
    }
}
