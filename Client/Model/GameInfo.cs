using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    class GameInfo
    {
        public DateTime StartTime { get; set; }
        public TimeSpan Length { get; set; }
        public int Place { get; set; }
        public List<Player> Enemies { get; set; }
    }
}
