using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Interfaces.ServerOptions
{
    interface ISocket
    {
        void StartSocketServer();
        void ClientConnection(System.Net.Sockets.Socket clientSocket);
    }
}
