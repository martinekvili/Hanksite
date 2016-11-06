using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    enum BotDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    static class BotDifficultyHelper
    {
        public static string convertToString(this BotDifficulty difficulty)
        {
            switch (difficulty)
            {
                case BotDifficulty.EASY:
                    return "Easy";
                case BotDifficulty.MEDIUM:
                    return "Medium";
                default:
                    return "Hard";
            }
        }

        public static BotDifficulty convertFromString(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    return BotDifficulty.EASY;
                case "Medium":
                    return BotDifficulty.MEDIUM;
                default:
                    return BotDifficulty.HARD;
            }
        }
    }
}
