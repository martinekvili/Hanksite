using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Player
{
    public class AIPlayer : PlayerBase
    {
        private readonly AIStrategy strategy;

        public AIPlayer(int id, GameManager game) : base(id, game)
        {
            this.strategy = new HardAIStrategy();
        }

        public override void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            Task.Factory.StartNew(() =>
            {
                var acquirableCellCounts = snapshot.Map.GetAcquirableCellCountsForPlayer(ID,
                    snapshot.AvailableCells.Select(cell => cell.Colour).Distinct());

                game.ChooseColour(this, strategy.ChooseColour(acquirableCellCounts));
            });
        }

        public override void SendGameOver(GameSnapshot snapshot)
        { }

        public override void SendGameSnapshot(GameSnapshot snapshot)
        { }
    }
}
