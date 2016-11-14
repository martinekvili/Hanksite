using Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Users;

namespace Server.Game.Player
{
    public class AIPlayer : PlayerBase
    {
        private readonly AIStrategy strategy;

        public AIPlayer(int botNumber, AIDifficulty type, GameManager game) : base(new User { ID = botNumber, UserName = $"AI{botNumber + 1}" }, game)
        {
            this.strategy = AIStrategyFactory.CreateAIStrategy(type);
        }

        public override void DoNextStep(List<Board.Hexagon> availableCells)
        {
            if (availableCells == null)
                return;

            Task.Factory.StartNew(() =>
            {
                var acquirableCellCounts = game.Map.GetAcquirableCellCountsForPlayer(ID,
                    availableCells.Select(cell => cell.Colour).Distinct());

                game.ChooseColour(this, strategy.ChooseColour(acquirableCellCounts));
            });
        }

        public override void SendGameOver()
        { }

        public override void SendGameSnapshot()
        { }

        public override void SendTimedOut()
        { }

        public override Common.Game.Player ToDto()
        {
            return new Common.Game.AIPlayer
            {
                Type = PlayerType.AI,
                User = user,
                Difficulty = strategy.Difficulty,
                Colour = Colour,
                Points = Points,
                Position = Position
            };
        }
    }
}
