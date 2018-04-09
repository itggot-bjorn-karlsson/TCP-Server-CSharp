using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Interfaces.ServerUser.SocketUser
{
    interface ISocketUser
    {
        Socket Client { get; set; }
    }
}
