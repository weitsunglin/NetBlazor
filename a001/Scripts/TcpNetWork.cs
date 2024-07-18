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
    private readonly WriteLog _writeLog;

    public TcpNetWork(WriteLog writeLog)
    {
        _listener = new TcpListener(IPAddress.Any, 5002);
        _cts = new CancellationTokenSource();
        _writeLog = writeLog;
        Console.WriteLine("TCP Server is being initialized on port 5002...");
        _writeLog.WriteLogEntry("TCP Server is being initialized on port 5002...");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _listener.Start();
        Console.WriteLine("TCP Server has started.");
        _writeLog.WriteLogEntry("TCP Server has started.");
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
                var clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                var clientInfo = $"Client connected: {clientEndPoint?.Address}:{clientEndPoint?.Port}";
                Console.WriteLine(clientInfo);
                _writeLog.WriteLogEntry(clientInfo);
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
                _writeLog.WriteLogEntry("Invalid message format.");
                break;
            }

            var protocolNumber = parts[0];
            var clientMessage = parts[1];
            var clientInfo = $"{clientEndPoint?.Address}:{clientEndPoint?.Port} - Protocol: {protocolNumber}, Received: {clientMessage}";
            Console.WriteLine(clientInfo);
            _writeLog.WriteLogEntry(clientInfo);

            var response = Encoding.UTF8.GetBytes($"Protocol: {protocolNumber}, Echo: {clientMessage}");
            await stream.WriteAsync(response, 0, response.Length, token);
        }

        Console.WriteLine("Client disconnected.");
        _writeLog.WriteLogEntry("Client disconnected.");
        client.Close();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        _listener.Stop();
        Console.WriteLine("TCP Server has stopped.");
        _writeLog.WriteLogEntry("TCP Server has stopped.");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _cts?.Dispose();
        _listener?.Stop();
    }
}
