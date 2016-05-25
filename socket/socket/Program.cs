using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace socket
{
    class Program
    {
        private static byte[] result = new byte[1024];
        private static int myProt = 8885;
        static Socket serverSocket;
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("192.168.0.104");
            serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip,myProt));
            serverSocket.Listen(10);
            Console.WriteLine("start to listen {0}",serverSocket.LocalEndPoint.ToString());

            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            Console.ReadLine();
        }

        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Send(Encoding.ASCII.GetBytes("Server say hello"));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }

        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    int receiveNum = myClientSocket.Receive(result);
                    Console.WriteLine("receive {0} from client {1}",myClientSocket.RemoteEndPoint.ToString(),Encoding.ASCII.GetString(result,0,receiveNum));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    throw;
                }
            }
        }
    }
}
