using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Interfaces.ServerUser.TCPUser
{
    interface ITCPUser
    {
        TcpClient TcpClient { get; set; }
        NetworkStream Sw { get; set; }
        NetworkStream Sr { get; set; }
    }
}
