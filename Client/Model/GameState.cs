using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class GameState
    {
        public List<Field> Map { get; set; }
        public List<GamePlayer> Players { get; set; }
    }
}
