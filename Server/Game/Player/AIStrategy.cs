using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Player
{
    interface AIStrategy
    {
        int ChooseColour(Dictionary<int, int> acquirableCellCounts);
    }

    public class HardAIStrategy : AIStrategy
    {
        public int ChooseColour(Dictionary<int, int> acquirableCellCounts)
        {
            return (from cellCount in acquirableCellCounts
                where cellCount.Value == acquirableCellCounts.Max(keyValuePair => keyValuePair.Value)
                select cellCount.Key).First();
        }
    }
}
