using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.View.Interfaces
{
    interface IPasswordProvider
    {
        string GetPassword();
    }

    interface IConfirmedPasswordProvider : IPasswordProvider
    {
        string GetConfirmedPassword();
    }
}
