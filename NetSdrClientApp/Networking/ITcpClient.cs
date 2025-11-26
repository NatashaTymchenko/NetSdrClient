uusing System.Threading.Tasks;
using System;

namespace NetSdrClientApp.Networking
{
    public interface ITcpClient : IDisposable
    {
        bool Connected { get; }
        Task ConnectAsync();
        void Disconnect();
        Task WriteAsync(byte[] data);
    }
}
