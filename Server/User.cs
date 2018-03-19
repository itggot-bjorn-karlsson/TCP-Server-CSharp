using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace tcp_server
{

    /// <summary>
    /// simple user holder for the server class, holds detail about each client
    /// </summary>
    public class User
    {
        public String Name { get; set; }            // name of client
        public Socket Client { get; set; }          // the socket 
        public Int32 ID { get; }                    // user id

        public TcpClient TcpClient { get; set; }    // TCP client (not working atm)
        public NetworkStream Sw { get; set; }       // network stream (writer) (not working atm)
        public NetworkStream Sr { get; set; }       // network stream (reader) (not working atm)

        public Boolean chatting = false;            // used for the chat program (not working atm)

        /// <summary>
        /// initialzie the user
        /// </summary>
        public User()
        {
            Random rand = new Random();
            ID = rand.Next(0, Int32.MaxValue);
        }
    }
}
