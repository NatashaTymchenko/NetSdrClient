using System.Threading.Tasks;

namespace NetSdrClientApp.Networking
{
    public interface ITcpClient: System.IDisposable
    {
       bool Connected { get; }
       Task ConnectAsync();
        void Disconnect();        
        Task WriteAsync(byte[] data);        
    }
}
