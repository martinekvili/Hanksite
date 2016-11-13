using Server.Game.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Player
{
    public class RealPlayer : PlayerBase
    {
        private readonly HanksiteSession session;

        public RealPlayer(HanksiteSession session, GameManager game) : base(session.User, game)
        {
            this.session = session;

            session.RealPlayer = this;
        }

        public void ChooseColour(int colour)
        {
            game.ChooseColour(this, colour);
        }

        public void DisconnectFromGame()
        {
            game.DisconnectPlayer(this);
        }

        public override void DoNextStep(List<Hexagon> availableCells)
        {
            var snapshot = game.GameSnapshot;

            session.DoNextStep(new Common.Game.GameSnapshotForNextPlayer
            {
                Map = snapshot.Map,
                Players = snapshot.Players,
                AvailableCells = availableCells.Select(cell => cell.Coord.ToDto()).ToArray()
            });
        }

        public override void SendGameOver()
        {
            session.SendGameOver(game.GameSnapshot);
        }

        public override void SendGameSnapshot()
        {
            session.SendGameSnapshot(game.GameSnapshot);
        }

        public override Common.Game.Player ToDto()
        {
            return new Common.Game.Player
            {
                User = user,
                Type = Common.Game.PlayerType.RealPlayer,
                Colour = CurrentColour,
                Points = Points,
                Position = CurrentPosition
            };
        }
    }
}
