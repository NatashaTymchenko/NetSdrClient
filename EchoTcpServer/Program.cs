using System;
using System.Threading.Tasks;

namespace EchoTcpServer
{
    class Program
    {
        static async Task Main(string[] args)
        {    
            var server = new EchoServer();
            server.Start(5000);
            string host = "127.0.0.1";
            int port = 60000;
            int intervalMilliseconds = 3000;
            using (var sender = new UdpTimedSender(host, port))
            {
                Console.WriteLine("Press any key to start sending UDP...");
                Console.ReadKey();
                
                sender.StartSending(intervalMilliseconds);

                Console.WriteLine("Press 'Q' to stop server and quit...");
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Q)
                {
                }

                sender.StopSending();
                server.Stop();
            }
        }
    }
}
