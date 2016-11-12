using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    interface IGameInfoProvider
    {
        List<GameInfo> GetGameInfos();
    }
}
