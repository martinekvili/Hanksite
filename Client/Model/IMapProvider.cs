using System.Collections.Generic;

namespace Client.Model
{
    interface IMapProvider
    {
        List<Field> GetMap();
    }
}
