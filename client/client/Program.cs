using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Program
    {
        private static byte[] result = new byte[1024];

        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("192.168.0.104");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip,8885));
                Console.WriteLine("connect successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("connect failed!");
                throw;
            }

            int receiveLength = clientSocket.Receive(result);
            Console.WriteLine("receive data from serve {0}",Encoding.ASCII.GetString(result,0,receiveLength));
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Thread.Sleep(1000);
                    string sendMessage = "client send Message Hellp"+DateTime.Now;
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    Console.WriteLine("send message to serve:{0}",sendMessage);
                }
                catch (Exception ex)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    throw;
                }
            }
            Console.WriteLine("over over!");
            Console.ReadLine();
        }


    }
}
