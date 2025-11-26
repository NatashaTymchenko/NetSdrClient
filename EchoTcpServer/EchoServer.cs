using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EchoTcpServer
{
    public class EchoServer
    {
        private TcpListener? _listener;
        private CancellationTokenSource? _cts;
     
        public int Port { get; private set; }
        public bool IsRunning { get; private set; }

        public void Start(int port)
        {
            if (IsRunning) return;

            Port = port;
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            IsRunning = true;
            _cts = new CancellationTokenSource();

            Console.WriteLine($"Server started on port {Port}.");

            Task.Run(() => ListenLoopAsync(_cts.Token));
        }

        public void Stop()
        {
            if (!IsRunning) return;

            _cts?.Cancel();
            _listener?.Stop();
            IsRunning = false;
            Console.WriteLine("Server stopped.");
        }

        private async Task ListenLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _listener != null)
                {
                    var client = await _listener.AcceptTcpClientAsync(token);
                    Console.WriteLine("Client connected.");
                    _ = HandleClientAsync(client, token);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"Listen error: {ex.Message}");
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            using (client)
            using (var stream = client.GetStream())
            {
                var buffer = new byte[8192];
                int bytesRead;
                try
                {
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token)) > 0)
                  
                        await stream.WriteAsync(buffer, 0, bytesRead, token);
                        Console.WriteLine($"Echoed {bytesRead} bytes.");
                    }
                }
                catch { /* Ігноруємо помилки при розриві з'єднання */ }
                finally { Console.WriteLine("Client disconnected."); }
            }
        }
    }
}
