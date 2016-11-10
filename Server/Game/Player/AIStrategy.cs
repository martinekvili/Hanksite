using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Utils;
using Common.Game;

namespace Server.Game.Player
{
    public static class AIStrategyFactory
    {
        public static AIStrategy CreateAIStrategy(AIDifficulty type)
        {
            switch (type)
            {
                case AIDifficulty.Easy:
                    return new EasyAIStrategy();
                case AIDifficulty.Medium:
                    return new MediumAIStrategy();
                case AIDifficulty.Hard:
                    return new HardAIStrategy();
                default:
                    throw new ArgumentException("Not recognized AIDifficulty");
            }
        }
    }

    public interface AIStrategy
    {
        int ChooseColour(Dictionary<int, int> acquirableCellCounts);
    }

    public class HardAIStrategy : AIStrategy
    {
        public int ChooseColour(Dictionary<int, int> acquirableCellCounts)
        {
            return (from cellCount in acquirableCellCounts
                    where cellCount.Value == acquirableCellCounts.Max(keyValuePair => keyValuePair.Value)
                    select cellCount.Key)
                    .First();
        }
    }

    public class MediumAIStrategy : AIStrategy
    {
        private static List<int> possibilities = new List<int> { 50, 25, 15, 5, 5 };

        public int ChooseColour(Dictionary<int, int> acquirableCellCounts)
        {
            return (from cellCount in acquirableCellCounts
                    orderby cellCount.Value
                    select cellCount.Key)
                    .ToList()
                    .GetRandomElementWithPossibilities(possibilities);
        }
    }

    public class EasyAIStrategy : AIStrategy
    {
        public int ChooseColour(Dictionary<int, int> acquirableCellCounts)
        {
            return acquirableCellCounts.Keys.ToList().GetRandomElement();
        }
    }
}
