using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;
using Microsoft.Extensions.Hosting;

namespace A001Model
{
    public class MessageObject
    {
        public string? Message { get; set; }
    }

    public class A001TcpNetwork : TcpNetWork
    {
        private readonly WriteLog _writeLog;

        public A001TcpNetwork(int port, WriteLog writeLog) : base(port)
        {
            _writeLog = writeLog;
        }

        public override async Task<byte[]> HandleMessage(string protocolNumber, string jsonMessage)
        {
            var messageObject = JsonSerializer.Deserialize<MessageObject>(jsonMessage);
            if (messageObject?.Message == null)
            {
                var errorMessage = $"Protocol: {protocolNumber}, Error: Message is null";
                Console.WriteLine(errorMessage);
                _writeLog.WriteLogEntry(errorMessage);
                return Encoding.UTF8.GetBytes(errorMessage);
            }

            var clientInfo = $"Protocol: {protocolNumber}, Received: {messageObject.Message}";
            Console.WriteLine(clientInfo);
            _writeLog.WriteLogEntry(clientInfo);

            var response = Encoding.UTF8.GetBytes($"Protocol: {protocolNumber}, Echo: {messageObject.Message}");
            return response;
        }

        protected override async Task AcceptClientsAsync(CancellationToken token)
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
    }
}