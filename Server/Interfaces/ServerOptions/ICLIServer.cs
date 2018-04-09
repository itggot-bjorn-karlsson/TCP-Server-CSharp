using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Interfaces.ServerOptions
{
    interface ICLIServer
    {
        void ServerConsoleHandler();
        String HandleRequest(RequestHandler RequestCommands, String RequestName, User user);
        String GetName(String message, Char location);
        List<String> GetArguments(String str);
        void InitiateClientCommandMethods();
        void InitiateServerCommandMethods();
        String Disconnect(Object user_);
        String GetTTL(Object user_);
        String GetIp();
        String Help();
        String Exit();
    }
}
