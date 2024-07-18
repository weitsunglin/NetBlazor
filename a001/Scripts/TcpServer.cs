using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class TcpNetWork : IHostedService, IDisposable
{
    private readonly TcpListener _listener;
    private CancellationTokenSource _cts;

    public TcpNetWork()
    {
        _listener = new TcpListener(IPAddress.Any, 5000);
        _cts = new CancellationTokenSource();
        Console.WriteLine("TCP Server is being initialized on port 5000...");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _listener.Start();
        Console.WriteLine("TCP Server has started.");
        _ = AcceptClientsAsync(_cts.Token);
        return Task.CompletedTask;
    }

    private async Task AcceptClientsAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");
                _ = HandleClientAsync(client, token);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is InvalidOperationException)
            {
                break;
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken token)
    {
        var buffer = new byte[1024];
        var stream = client.GetStream();

        while (!token.IsCancellationRequested)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
            if (bytesRead == 0) break;

            var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {message}");

            var response = Encoding.UTF8.GetBytes($"Echo: {message}");
            await stream.WriteAsync(response, 0, response.Length, token);
        }

        Console.WriteLine("Client disconnected.");
        client.Close();
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