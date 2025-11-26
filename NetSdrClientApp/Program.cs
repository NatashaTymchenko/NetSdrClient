using System;
using System.Threading.Tasks;
using NetSdrClientApp.Networking;

namespace NetSdrClientApp
{
    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Client started...");
            var tcpClient = new TcpClientWrapper("127.0.0.1", 5000);
            var udpClient = new UdpClientWrapper(60000);
            var netSdr = new NetSdrClient(tcpClient, udpClient);

            await netSdr.ConnectAsync();
            netSdr.Disconnect();
        }
    }
}
