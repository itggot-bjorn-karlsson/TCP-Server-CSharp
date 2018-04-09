using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Interfaces.ServerOptions
{
    interface IServerRequest
    {
        Dictionary<String, Object> DictionaryRequests { get; set; }

        Dictionary<String, Func<Object>> RequestHolder_1 { get; set; }
        Dictionary<String, Func<Object, Object>> RequestHolder_2 { get; set; }
        Dictionary<String, Func<Object, Object, Object>> RequestHolder_3 { get; set; }
        Dictionary<String, Func<Object, Object, Object, Object>> RequestHolder_4 { get; set; }
        Dictionary<String, Func<Object, Object, Object, Object, Object>> RequestHolder_5 { get; set; }
        Dictionary<String, Func<Object, Object, Object, Object, Object, Object>> RequestHolder_6 { get; set; }
        Dictionary<String, String> Discriptions { get; set; }

        List<String> ArgKeyNames { get; set; }
        List<String> Keys { get; set; }
        List<String> KeysName { get; set; }
        String ClassName { get; set; }

        void AddMethodOnRequest(String Name, Func<Object> RequestMethod, String Discription);
        void AddMethodOnRequest(String Name, Func<Object, Object> RequestMethod, String Discription);
        void AddMethodOnRequest(String Name, Func<Object, Object, Object> RequestMethod, String Discription);
        void AddMethodOnRequest(String Name, Func<Object, Object, Object, Object> RequestMethod, String Discription);
        void AddMethodOnRequest(String Name, Func<Object, Object, Object, Object, Object> RequestMethod, String Discription);
        void AddMethodOnRequest(String Name, Func<Object, Object, Object, Object, Object, Object> RequestMethod, String Discription);

        Object TryRequestByName(String Name);
        Object TryRequestByName(String Name, Object Arg1);
        Object TryRequestByName(String Name, Object Arg1, Object Arg2);
        Object TryRequestByName(String Name, Object Arg1, Object Arg2, Object Arg3);
        Object TryRequestByName(String Name, Object Arg1, Object Arg2, Object Arg3, Object Arg4);
        Object TryRequestByName(String Name, Object Arg1, Object Arg2, Object Arg3, Object Arg4, Object Arg5);
    }
}
