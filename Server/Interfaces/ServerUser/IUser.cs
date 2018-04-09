using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Interfaces.ServerUser
{
    interface IUser
    {
        String Name { get; set; }
        Int32 ID { get; }
    }
}
