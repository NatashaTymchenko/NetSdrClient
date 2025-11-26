using System.Threading.Tasks;

namespace NetSdrClientApp.Networking
{﻿
    public interface IUdpClient
    {
        Task StartListeningAsync();
        void StopListening();   
    }
}
