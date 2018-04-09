using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Interfaces.ServerOptions
{
    interface IServerStandard
    {
        String ChooseIP();

        void Start();
        void UserUpdater();
    }
}
