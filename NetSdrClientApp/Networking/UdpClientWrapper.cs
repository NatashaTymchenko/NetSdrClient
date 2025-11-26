using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NetSdrClientApp.Networking
{
    public class UdpClientWrapper : IUdpClient, IDisposable
    {
        private readonly IPEndPoint _localEndPoint;
        private CancellationTokenSource? _cts;
        private UdpClient? _udpClient;

        public event EventHandler<byte[]>? MessageReceived;

        public UdpClientWrapper(int port)
        {
            _localEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        public async Task StartListeningAsync()
        {
            if (_udpClient != null) return;

            _cts = new CancellationTokenSource();
            _udpClient = new UdpClient(_localEndPoint);

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    var result = await _udpClient.ReceiveAsync(_cts.Token);
                    MessageReceived?.Invoke(this, result.Buffer);
                }
            }
            catch (Exception) { }
        }

        public void StopListening()
        {
            _cts?.Cancel();
            _udpClient?.Close();
            _udpClient = null;
        }

        public void Dispose()
        {
            StopListening();
            _cts?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
