using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetSdrClientApp.Networking
{
    public class TcpClientWrapper : ITcpClient
    {
        private readonly TcpClient? _tcpClient;
        private NetworkStream? _stream;       

        public bool Connected => _tcpClient.Connected;

        public TcpClientWrapper(string host, int port)
        {
            _tcpClient = new TcpClient(host, port);
            _stream = _tcpClient.GetStream();
        }

        public async Task ConnectAsync()
        {
            if (Connected) return; 
            await Task.CompletedTask;
        }

       public void Disconnect() => _tcpClient.Close();

        public async Task WriteAsync(byte[] data)
        {
            if (_stream != null)
            {
                await _stream.WriteAsync(data, 0, data.Length);
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _tcpClient.Dispose();
            GC.SuppressFinalize(this);
        }
        
    }

}
