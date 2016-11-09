namespace Client.Model
{
    enum BotDifficulty
    {
        EASY,
        MEDIUM,
        HARD,
        INVALID
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
                case BotDifficulty.HARD:
                    return "Hard";
                default:
                    return "";
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
                case "Hard":
                    return BotDifficulty.HARD;
                default:
                    return BotDifficulty.INVALID;
            }
        }
    }
}
