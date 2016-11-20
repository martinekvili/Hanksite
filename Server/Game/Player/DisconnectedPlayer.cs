using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Board;
using Common.Users;

namespace Server.Game.Player
{
    public class DisconnectedPlayer : PlayerBase
    {
        public DisconnectedPlayer(User user, GameManager game) : base(user, game)
        { }

        public DisconnectedPlayer(RealPlayer player) : base(player)
        { }

        public override bool CanDoStep => false;

        public override bool IsReady => true;

        public override void DoNextStep(List<Hexagon> availableCells)
        { }

        public override void SendGameOver()
        { }

        public override void SendGameSnapshot()
        { }

        public override Common.Game.Player ToDto()
        {
            return new Common.Game.Player
            {
                User = user,
                Type = Common.Game.PlayerType.DisconnectedPlayer,
                Colour = Colour,
                Points = Points,
                Position = Position
            };
        }

        public override void SendGamePlayersSnapshot()
        { }
    }
}
