using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync("localhost", 5002);
            Console.WriteLine("Connected to server.");

            using var networkStream = client.GetStream();
            string protocolNumber = "10"; // 协议编号从10开始
            string message = "Hello, Server!";
            string fullMessage = $"{protocolNumber}|{message}";
            byte[] data = Encoding.UTF8.GetBytes(fullMessage);

            await networkStream.WriteAsync(data, 0, data.Length);
            Console.WriteLine($"Sent: {fullMessage}");

            byte[] buffer = new byte[1024];
            int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {response}");

            client.Close();
            Console.WriteLine("Connection closed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
