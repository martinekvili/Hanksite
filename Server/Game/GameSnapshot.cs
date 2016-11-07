using Server.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Board;

namespace Server.Game
{
    public class GameSnapshot
    {
        public readonly List<PlayerBase> Players;
        public readonly Map Map;

        public GameSnapshot(List<PlayerBase> players, Map map)
        {
            this.Players = players;
            this.Map = map;
        }
    }

    public class GameSnapshotForNextPlayer : GameSnapshot
    {
        public readonly List<Hexagon> AvailableCells;

        public GameSnapshotForNextPlayer(List<PlayerBase> players, Map map, List<Hexagon> availableCells) : base(players, map)
        {
            this.AvailableCells = availableCells;
        }
    }
}
