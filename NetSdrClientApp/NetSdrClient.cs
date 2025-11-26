using NetSdrClientApp.Networking;
using System;
using System.Threading.Tasks;

namespace NetSdrClientApp
{
    public class NetSdrClient
    {
        private readonly ITcpClient _tcpClient;
        private readonly IUdpClient _udpClient;

        public bool IQStarted { get; private set; }

        public NetSdrClient(ITcpClient tcpClient, IUdpClient udpClient)
        {
            _tcpClient = tcpClient;
            _udpClient = udpClient;
        }

        // Оновлений виклик асинхронного методу
        public async Task ConnectAsync() => await _tcpClient.ConnectAsync();

        public void Disconnect()
        {
            if (IQStarted) _ = StopIQAsync();
            _tcpClient.Disconnect();
        }

        public async Task StartIQAsync()
        {
            if (IQStarted) return;

            await SendCommandAsync(0x0080, 0x00); 
            
            await _udpClient.StartListeningAsync();
            IQStarted = true;
        }

        public async Task StopIQAsync()
        {
            if (!IQStarted) return;

            await SendCommandAsync(0x0000, 0x00);

            _udpClient.StopListening();
            IQStarted = false;
        }

        public async Task ChangeFrequencyAsync(long frequencyHz, byte channel)
        {
            byte[] freqBytes = BitConverter.GetBytes(frequencyHz);
            await SendCommandAsync(0x0020, channel, freqBytes);
        }

        private async Task SendCommandAsync(ushort command, byte channel, byte[]? data = null)
        {
            ushort dataLen = (ushort)(data?.Length ?? 0);
            ushort length = (ushort)(2 + 2 + 1 + dataLen);
            
            byte[] packet = new byte[length];

            packet[0] = (byte)(length & 0xFF);
            packet[1] = (byte)((length >> 8) & 0xFF);
            packet[2] = (byte)(command & 0xFF);
            packet[3] = (byte)((command >> 8) & 0xFF);
            packet[4] = channel;

            if (data != null && dataLen > 0)
            {
                int bytesToCopy = Math.Min(data.Length, length - 5);
                Array.Copy(data, 0, packet, 5, bytesToCopy);
            }

            // Оновлений виклик WriteAsync
            await _tcpClient.WriteAsync(packet);
        }
    }
}
