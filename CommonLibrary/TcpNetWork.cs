using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace CommonLibrary
{
    public class TcpNetWork : IHostedService, IDisposable
    {
        protected readonly TcpListener _listener;
        private CancellationTokenSource _cts;
        private readonly int _port;

        public TcpNetWork(int port = 5002)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _cts = new CancellationTokenSource();
            _port = port;
            Console.WriteLine($"TCP Server is being initialized on port {port}...");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _listener.Start();
            Console.WriteLine("TCP Server has started.");
            _ = AcceptClientsAsync(_cts.Token);
            return Task.CompletedTask;
        }

        protected virtual async Task AcceptClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    var clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                    var clientInfo = $"Client connected: {clientEndPoint?.Address}:{clientEndPoint?.Port}";
                    Console.WriteLine(clientInfo);
                    _ = HandleClientAsync(client, token);
                }
                catch (Exception ex) when (ex is ObjectDisposedException || ex is InvalidOperationException)
                {
                    break;
                }
            }
        }

        protected virtual async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            var buffer = new byte[1024];
            var stream = client.GetStream();
            var clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;

            while (!token.IsCancellationRequested)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                if (bytesRead == 0) break;

                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var parts = message.Split('|');
                if (parts.Length < 2)
                {
                    Console.WriteLine("Invalid message format.");
                    break;
                }

                var protocolNumber = parts[0];
                var jsonMessage = parts[1];

                try
                {
                    var response = await HandleMessage(protocolNumber, jsonMessage);
                    await stream.WriteAsync(response, 0, response.Length, token);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Failed to deserialize JSON message: {ex.Message}");
                }
            }

            Console.WriteLine("Client disconnected.");
            client.Close();
        }

        public virtual Task<byte[]> HandleMessage(string protocolNumber, string jsonMessage)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _listener.Stop();
            Console.WriteLine("TCP Server has stopped.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _listener?.Stop();
        }
    }
}
