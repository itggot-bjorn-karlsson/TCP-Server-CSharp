



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace tcp_server
{
    /// <summary>
    /// The main <c>Server</c> class.
    /// Contains every method used for client and server connection using a basic CLI.
    /// Multithreaded.
    /// </summary>
    class Server : Interfaces.ServerOptions.ISocket, Interfaces.ServerOptions.IServerStandard, Interfaces.ServerOptions.ICLIServer
    {
        private static RequestHandler ClientCommands;                   // Holds every command for clientsided requests.
        private static RequestHandler ServerCommands;                   // Holds every command for serversided requests.
        private static String IP;                                       // The IPAddress of the Host.
        private static String ServerStartedTime;                        // Holds server startup time.
        private static List<User> Users = new List<User>();             // Conatins all the users connected to the server
        private static List<String> BannedNames = new List<String>();   // Holds every name from banned users

        /// <summary>
        /// Executes the program.
        /// </summary>
        public void Start()
        {
            // Testing
            StartSocketServer();
            // Goal
            // StartTcpServer();
        }

        /// <summary>
        /// Give the host an opportunity to choose between different IP configurations. 
        /// </summary>
        /// <returns>
        /// Returns the chosen IP Address in a String
        /// </returns>
        public String ChooseIP()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The server found multiple Network interfaceses");
            Dictionary<String, IPAddress> IPByIndex = new Dictionary<string, IPAddress>();
            Int32 AdressesLength = Dns.GetHostByName(Dns.GetHostName()).AddressList.Length;
            IPAddress[] Adresses = Dns.GetHostByName(Dns.GetHostName()).AddressList;

            int i = 0;
            foreach (IPAddress ip in Adresses)
            {
                Console.WriteLine("[" + i + "] :: " + ip.ToString());
                IPByIndex.Add(i.ToString(), ip);
                i += 1;
            }

            Console.Write("Pick one IP Address by index: ");
            String key = Console.ReadLine();
            return IPByIndex[key].ToString();
        }
        /// <summary>
        /// Initiates the server using a Socket
        /// </summary>
        public void StartSocketServer()
        {
            String hostName = Dns.GetHostName(); // Retrive the Name of HOST 
            if(Dns.GetHostByName(hostName).AddressList.Length > 1)
            {
                IP = ChooseIP();
            }else
            {
                IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            }
            
            start:
            ClientCommands = new RequestHandler("Client");
            ServerCommands = new RequestHandler("Server");
            System.Net.Sockets.Socket listenerSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEnd;
            try
            {
                ipEnd = new IPEndPoint(IPAddress.Parse(IP), 3000);
                listenerSocket.Bind(ipEnd);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Server initialized with IP: " + IP);
                Console.WriteLine("Server started...");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Loading client commands");
                InitiateClientCommandMethods();
                Console.WriteLine("Loading Server commands");
                InitiateServerCommandMethods();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Server loading complete...");
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server got wrong IP address: " + IP);
                Console.Write("Enter new IP address: ");
                IP = Console.ReadLine();
                Console.WriteLine(IP);
                Console.Clear();
                goto start;
            }
            ServerStartedTime = DateTime.Now.ToString();
            Console.WriteLine("Time: " + ServerStartedTime);
            Console.ForegroundColor = ConsoleColor.Gray;
            Thread serverManager;
            Thread UserManager;
            serverManager = new Thread(() => ServerConsoleHandler());
            serverManager.Start();
            UserManager = new Thread(() => UserUpdater());
            UserManager.Start();

            while (true)
            {
                listenerSocket.Listen(0);
                System.Net.Sockets.Socket clientSocket = listenerSocket.Accept();

                // Threads
                Thread clientThread;
                clientThread = new Thread(() => ClientConnection(clientSocket));
                clientThread.Start();
            }
        }
        /// <summary>
        /// A method used in a different Thread. Used for CLI inputs on the Host
        /// </summary>
        public void ServerConsoleHandler()
        {
            while (true)
            {
                String adminRequest = Console.ReadLine();
                String message = HandleRequest(ServerCommands, adminRequest, null);
                if (message != String.Empty)
                {
                    Console.WriteLine(message);
                }
            }
        }
        /// <summary>
        /// A method used in a different Thread. Used for updating clients on the server
        /// </summary>
        /// <Exception = "System.InvalidOperationException"> Catches errors when updating the Users List
        public void UserUpdater()
        {
            while (true)
            {
                Thread.Sleep(100);
                try
                {
                    foreach (User user in Users)
                    {
                        if (user.Client.Connected == false)
                        {
                            Users.Remove(user);
                        }
                    }
                }
                catch (InvalidOperationException)
                {

                }
            }
        }
        /// <summary>
        /// This method will execute in x amout of Threads depending on how many clients connect to the server.
        /// Will create a session for the client and the server.
        /// Handles request and responses
        /// </summary>
        /// <param name="clientSocket">The client socket is used for multithreaded connections, Taken from an earlier stage of the program </param>
        public void ClientConnection(System.Net.Sockets.Socket clientSocket)
        {
            User ClientUser = new User();
            ClientUser.Client = clientSocket;
            Byte[] Buffer;
            Int32 ReadByte;
            String Data = "";
            String ClientRequest = "";
            String Name = "unknown";
            Int32 NameLocation;
            bool ReadName = true;
            try
            {
                do
                {
                    Buffer = new Byte[2024];
                    // Receive
                    ReadByte = clientSocket.Receive(Buffer);
                    // do stuff
                    Byte[] rData = new Byte[ReadByte];
                    Array.Copy(Buffer, rData, ReadByte);
                    Data = Encoding.UTF8.GetString(rData);
                    if (ReadName)
                    {
                        Name = GetName(Data, ';');
                        if (BannedNames.Contains(Name.ToLower()))
                        {
                            String BanMsg = "You have been banned from the server";
                            clientSocket.Send(Encoding.UTF8.GetBytes(BanMsg));
                            clientSocket.Close();
                            Console.WriteLine("User tried to connect but is still banned: " + Name);
                            break;
                        }
                        Console.WriteLine("Client connected!");
                        ClientUser.Name = Name;
                        Users.Add(ClientUser);
                        ReadName = !ReadName;
                    }
                    NameLocation = Name.Length + 1;
                    ClientRequest = Data.Remove(0, NameLocation);
                    Console.WriteLine("Got request from (" + Name + ") command: " + ClientRequest);
                    // SendBack data
                    String message = "";
                    message = HandleRequest(ClientCommands, ClientRequest, ClientUser);
                    clientSocket.SendBufferSize = message.Length;
                    clientSocket.Send(Encoding.UTF8.GetBytes(message));
                } while (ReadByte > 0);

                Console.WriteLine("Client disconnected : (" + Name + ")");
                clientSocket.Close();
            }
            catch (SocketException)
            {
                Console.WriteLine("Client disconnected : (" + Name + ")");
                clientSocket.Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                if (Name.Length == 0)
                {
                    Console.WriteLine("Client disconnected : (unknown)");
                    clientSocket.Close();
                }
                else
                {
                    Console.WriteLine("Client disconnected : (" + Name + ")");
                    clientSocket.Close();
                }

            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Client disconnected : (" + Name + ")");
                clientSocket.Close();
            }
        }
        /// <summary>
        /// This method will try to execute any command it finds in the server/client request class
        /// </summary>
        /// <param "RequestCommands">Checks wether the later command is for the server or the client</param>
        /// <param "RequestName">The type of command requested to execute</param>
        /// <param "user">The requesters User information (Only if client)</param>
        /// <returns>
        /// Returns the recived value from the executed command
        /// </returns>
        /// <example>
        /// [CLI]
        /// ::Users
        /// (1)Carl
        /// (2)Josef
        /// </example>
        public String HandleRequest(RequestHandler RequestCommands, String RequestName, User user)
        {
            RequestName = RequestName.ToLower();
            RequestName = RequestName.Replace(" ", String.Empty);
            String Name = GetName(RequestName, '(');
            String msg;
            List<String> arguments = GetArguments(RequestName);
            if (RequestCommands.HasArg(Name) == true)
            {

                if (arguments.Count() == 1)
                {

                    msg = (String)RequestCommands.TryRequestByName(Name, arguments[0]);
                    if (msg != "Invalid command")
                    {
                        return msg;
                    }
                }
                if (arguments.Count() == 2)
                {
                    msg = (String)RequestCommands.TryRequestByName(Name, arguments[0], arguments[1]);
                    if (msg != "Invalid command")
                    {
                        return msg;
                    }
                }
                if (arguments.Count() == 0)
                {
                    msg = (String)RequestCommands.TryRequestByName(Name, user);
                    if (msg != "Invalid command")
                    {
                        return msg;
                    }
                }

                if (arguments.Count() == 1)
                {
                    msg = (String)RequestCommands.TryRequestByName(Name, user, arguments[0]);
                    if (msg != "Invalid command")
                    {
                        return msg;
                    }
                }
                if (arguments.Count() == 2)
                {
                    msg = (String)RequestCommands.TryRequestByName(Name, user, arguments[0], arguments[1]);
                    if (msg != "Invalid command")
                    {
                        return msg;
                    }
                }
            }

            if (RequestCommands.HasArg(Name) == false)
            {
                msg = (String)RequestCommands.TryRequestByName(Name);
                if (msg != "Invalid command")
                {
                    return msg;
                }
            }
            if (RequestCommands.HasArg(Name) == false)
            {
                msg = (String)RequestCommands.TryRequestByName(Name, user);
                if (msg != "Invalid command")
                {
                    return msg;
                }
            }
            return "Invalid command";
        }
        /// <summary>
        /// This is a copy of the method Substring in the Class String
        /// </summary>
        /// <param "message">Takes a string</param>
        /// <param "location">First location</param>
        /// <returns>
        /// Returns the substring of the message by location
        /// </returns>
        public String GetName(String message, Char location)
        {
            String Name = "";
            foreach (char c in message)
            {
                if (c == location)
                {
                    break;
                }
                else
                {
                    Name += c;
                }
            }
            return Name;
        }
        /// <summary>
        /// Custom Method that find the arguments in a string add(1,2) #=> [1,2]
        /// </summary>
        /// <param "str">The string with the arguments</param>
        /// <returns>
        /// Returns the arguments in a array of strings
        /// </returns>
        public List<String> GetArguments(String str)
        {
            List<String> Arguments = new List<String>();
            Int32 i = 0;
            String Arg = "";
            while (i < str.Length)
            {
                if (str[i] == '(')
                {
                    int j = i + 1;

                    while (j < str.Length)
                    {
                        if (str[j] != ',' && str[j] != ')')
                        {
                            Arg += str[j];
                        }
                        else
                        {
                            Arguments.Add(Arg);
                            Arg = "";

                        }

                        if (str[j] == ')')
                        {
                            i = j;
                            break;
                        }
                        j += 1;
                    }
                }

                i += 1;
            }
            return Arguments;
        }
        /// <summary>
        /// Initiates all client commands that can only be used from the Client CLI
        /// </summary>
        public void InitiateClientCommandMethods()
        {
            ClientCommands.AddMethodOnRequest("help", Help,             "Displays all commands:----------------------::::::::::::::::::::: Syntax: help");
            ClientCommands.AddMethodOnRequest("quit", Disconnect,       "Disconnect you from the server:-------------::::::::::::::::::::: Syntax: quit");
            ClientCommands.AddMethodOnRequest("ip", GetIp,              "Recives the IP from the server:-------------::::::::::::::::::::: Syntax: ip");
            ClientCommands.AddMethodOnRequest("ttl", GetTTL,            "Gets the TTL information:-------------------::::::::::::::::::::: Syntax: ttl");
        }
        /// <summary>
        /// Initiates all client commands that can only be used from the Server CLI
        /// </summary>
        public void InitiateServerCommandMethods()
        {
            ServerCommands.AddMethodOnRequest("help", Help_Sv,                  "Displays all commands:---------------------:::::::::::::::::::: Syntax: help");
            ServerCommands.AddMethodOnRequest("cls", Clear,                     "Clears the screen:-------------------------:::::::::::::::::::: Syntax: cls");
            ServerCommands.AddMethodOnRequest("time", GetTime,                  "Gets the current time:---------------------:::::::::::::::::::: Syntax: time");
            ServerCommands.AddMethodOnRequest("timeserver", GetServerStartTime, "recives the time the server started at:----:::::::::::::::::::: Syntax: timeserver");
            ServerCommands.AddMethodOnRequest("exit", Exit,                     "Exits the server:--------------------------:::::::::::::::::::: Syntax: exit");
            ServerCommands.AddMethodOnRequest("users", GetUsers,                "Recives all the users:---------------------:::::::::::::::::::: Syntax: users");
            ServerCommands.AddMethodOnRequest("kick", KickByName,               "Kicks a user by name:----------------------:::::::::::::::::::: Syntax: kick(name)");
            ServerCommands.AddMethodOnRequest("ban", BanByName,                 "Banns a user by name:----------------------:::::::::::::::::::: Syntax: ban(name)");
            ServerCommands.AddMethodOnRequest("ipconfig", IpConfig,             "Returns details about the host:------------:::::::::::::::::::: Syntax: ipconfig");
        }
        #region Request Methods
        // test, read the name and you will understand
        public String Addition(Object n1, Object n2)
        {

            try
            {
                Double number1 = Convert.ToDouble(n1);
                Double number2 = Convert.ToDouble(n2);
                Double num = number1 + number2;
                String answear = n1 + " + " + n2 + " = " + num.ToString();
                return answear;
            }
            catch (FormatException)
            {
                return "Syntax error";
            }
        }
        public String Subtraction(Object n1, Object n2)
        {
            try
            {
                Double number1 = Convert.ToDouble(n1);
                Double number2 = Convert.ToDouble(n2);
                Double num = number1 - number2;
                String answear = n1 + " - " + n2 + " = " + num.ToString();
                return answear;
            }
            catch (FormatException)
            {
                return "Syntax error";
            }

        }
        public String Division(Object n1, Object n2)
        {
            try
            {
                Int32 number1 = Convert.ToInt32(n1);
                Int32 number2 = Convert.ToInt32(n2);
                Int32 num = number1 / number2;
                String answear = n1 + " / " + n2 + " = " + num.ToString();
                return answear;
            }
            catch (FormatException)
            {
                return "Syntax error";
            }
            catch (DivideByZeroException)
            {
                return "You cant divide by zero";
            }

        }
        public String Multiplication(Object n1, Object n2)
        {
            try
            {
                Double number1 = Convert.ToDouble(n1);
                Double number2 = Convert.ToDouble(n2);
                Double num = number1 * number2;
                String answear = n1 + " * " + n2 + " = " + num.ToString();
                return answear;
            }
            catch (FormatException)
            {
                return "Syntax error";
            }
        }
        public String SqrtOf(Object n1)
        {
            try
            {
                Double number1 = Convert.ToDouble(n1);
                String answear = "The square root of " + number1 + " = " + Math.Sqrt(number1);
                return answear;
            }
            catch (FormatException)
            {
                return "Syntax error";
            }
            catch (InvalidCastException)
            {
                return "Syntax error";
            }
        }

        /// <summary>
        /// (Client command)Disconnects from the current server 
        /// </summary>
        /// <param user="user_">Current user that will get disconnected</param>
        /// <returns> returns the information about the command (did it work or not)</returns>
        public String Disconnect(Object user_)
        {
            User user = (User)user_;
            System.Net.Sockets.Socket client = user.Client;
            client.Close();
            client.Dispose();
            return "Disconnected";
        }
        /// <summary>
        /// (Client command)gets the TTL value from the client
        /// </summary>
        /// <param user="user_">The user with the TTL </param>
        /// <returns>returns the TTL of the user</returns>
        public String GetTTL(Object user_)
        {
            User user = (User)user_;
            System.Net.Sockets.Socket client = user.Client;
            return client.Ttl.ToString();
        }
        /// <summary>
        /// (Client command)reicves the IP address that the server uses
        /// </summary>
        /// <returns>returns the IP as a string</returns>
        public String GetIp()
        {
            return IP;
        }
        /// <summary>
        /// (Client command) gets all the information about the commands
        /// </summary>
        /// <returns>returns information as a string</returns>
        public String Help()
        {
            return ClientCommands.GetAllRequest();
        }
        /// <summary>
        /// (Server command) gets all the information about the commands
        /// </summary>
        /// <returns>returns information as a string</returns>
        public String Help_Sv()
        {
            return ServerCommands.GetAllRequest();
        }
        /// <summary>
        /// (Server command) gets the date and time when the server started
        /// </summary>
        /// <returns>returns time as a string</returns>
        public String GetServerStartTime()
        {
            return ServerStartedTime;
        }
        /// <summary>
        /// (Server command) gets the current time
        /// </summary>
        /// <returns>returns the time as a string</returns>
        public String GetTime()
        {
            return DateTime.Now.ToString();
        }
        /// <summary>
        /// (server command) clear the CLI from any text
        /// </summary>
        /// <returns>returns errormessages </returns>
        public String Clear()
        {
            Console.Clear();
            return String.Empty;
        }
        /// <summary>
        /// (server command) exits the server 
        /// </summary>
        /// <returns>data about the shutdown</returns>
        public String Exit()
        {
            Process p = Process.GetCurrentProcess();
            p.Kill();
            return "";
        }
        /// <summary>
        /// (server command)recives a list of all users that is currently connected to the server
        /// </summary>
        /// <returns> returns a string of all users</returns>
        public String GetUsers()
        {
            String msg = String.Empty;
            Int32 i = 1;
            foreach (User user in Users)
            {
                msg += "(" + i + ") :" + user.Name + "\n";
                i += 1;
            }
            if (msg == String.Empty)
            {
                return "No user connected";
            }
            else
            {
                return msg;
            }
        }
        /// <summary>
        /// (server command) kicks a user by a name
        /// </summary>
        /// <param name="name_"> the name of the user you want to kick</param>
        /// <returns>any error messages or nothing</returns>
        public String KickByName(Object name_)
        {
            String name = (String)name_;
            foreach (User user in Users)
            {
                if (user.Name.ToLower() == name.ToLower())
                {
                    user.Client.Close();
                    Users.Remove(user);
                    return "Kicked (" + name + ") from the server";
                }
            }
            return "User not found";
        }
        /// <summary>
        /// (server command) ban a user, that user may never connect again
        /// </summary>
        /// <param name="name_"> the name of the user you want to ban</param>
        /// <returns>error messages</returns>
        public String BanByName(Object name_)
        {
            String name = (String)name_;
            foreach (User user in Users)
            {
                if (user.Name.ToLower() == name.ToLower())
                {
                    user.Client.Close();
                    Users.Remove(user);
                    BannedNames.Add(name);
                    return "Banned (" + name + ") from the server";
                }
            }

            return "User not found";
        }
        /// <summary>
        /// (server command) get details about the network interface
        /// </summary>
        /// <returns>error message or info as a string</returns>
        public String IpConfig()
        {
            return "Hostname: " + Dns.GetHostName() + "\nIPv4: " + IP;
        }

        #endregion
    }

    class Test
    {
        static void Main()
        {
            Server sv = new Server();
            sv.Start();
        }
    }
}







