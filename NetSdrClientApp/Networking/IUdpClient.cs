using System.Threading.Tasks;
using System;

namespace NetSdrClientApp.Networking
{
    public interface IUdpClient : IDisposable
    {
        Task StartListeningAsync();
        void StopListening();
    }
}
