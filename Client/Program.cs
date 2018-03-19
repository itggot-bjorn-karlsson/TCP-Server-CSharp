using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace tcp_client
{

    // socketclient
    class Client
    {
        static void Test()
        {
            String hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            String IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            IP = "192.168.195.20";
            TcpClient client = new TcpClient(IP, 3000);
            NetworkStream sr = client.GetStream();
            NetworkStream sw = client.GetStream();
        }
        static void Main(string[] args)
        {
            bool running = true;
            String Name = ";";
            run:

            if (running)
            {
                do
                {
                    Console.Clear();
                    Console.Write("Enter your name: ");
                    // get name
                    Name = Console.ReadLine();
                    Name += ";";
                } while (Name == ";");
                connect:
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    String hostName = Dns.GetHostName();
                    //String IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                    String IP = "192.168.195.20";
                    IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(IP), 3000);
                    client.Connect(ipEnd);

                    byte[] bytes;

                    String sendData = "";
                    do
                    {
                        // send data
                        Console.Write("Send: ");
                        sendData = Name;
                        sendData += Console.ReadLine();
                        String checkMsg = sendData.ToLower();
                        bytes = new byte[2024];
                        if (checkMsg.Contains("exit"))
                        {
                            client.Close();
                            running = false;
                            goto run;
                        }
                        // BitConverter.ToChar
                        client.Send(Encoding.UTF8.GetBytes(sendData));
                        // get data
                        Console.Clear();
                        client.Receive(bytes);

                        String msg = Encoding.UTF8.GetString(bytes);

                        for (int i = 0; i < msg.Length; i++)
                        {

                            if (bytes[i] == 00)
                            {
                                msg = msg.Substring(0, i);
                                break;
                            }
                        }
                        if(msg == "chatting=true")
                        {
                            StartChat();
                        }
                        Console.WriteLine(msg);

                    } while (sendData.Length > 0);


                    client.Close();
                }
                catch (SocketException)
                {
                    Console.WriteLine("Could not connect to the server");
                    client.Close();
                    Thread.Sleep(1000);
                    goto connect;
                }
            }
        }
        static void StartChat()
        {
            while (true)
            {
                // thread reader 
                // thread sender 

            }
        }
    }
}
