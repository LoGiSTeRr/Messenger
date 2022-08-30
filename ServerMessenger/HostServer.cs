using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatModelLibrary;
using ChatModelLibrary.Messages;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServerMessenger;

public class HostServer
{
    private int _id;
    private Socket _server;
    private IPAddress _ipAddress;
    private IPEndPoint _endPoint;
    private ConcurrentBag<Client> _chatClients;

    public HostServer()
    {
        _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _ipAddress = IPAddress.Parse("127.0.0.1");
        _endPoint = new IPEndPoint(_ipAddress, 44433);
        _chatClients = new ConcurrentBag<Client>();
    }

    public void Launch()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken ct = cts.Token;
        Task.Factory.StartNew(() =>
        {
            _server.Bind(_endPoint);
            _server.Listen();

            Console.WriteLine("Server started");

            while (true)
            {
                Console.WriteLine("Waiting for client");
                
                Client clientConnected = new Client(_server.Accept());
                
                _chatClients.Add(clientConnected);

                Task.Factory.StartNew(() =>
                {
                    if (!_chatClients.TryTake(out Client? client))
                    {
                        return;
                    }
                    
                    Console.WriteLine("Socket connected");

                    byte[] messageBuffer = new byte[2048];

                    try
                    {
                        while (client.ClientSocket.Connected)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                ct.ThrowIfCancellationRequested();
                            }
                            client.ClientSocket.Receive(messageBuffer);
                            Console.WriteLine("Message Received");
                            ReceiveMessage(client, messageBuffer.ToString()!);
                        }
                    }
                    catch (Exception ex) when (ex is SocketException or ObjectDisposedException)
                    {
                        Console.WriteLine("Socket disconnected");
                    }
                    finally
                    {
                        Console.WriteLine("Socket disconnected");
                        client.ClientSocket.Dispose();
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }, ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }
    
    private void ReceiveMessage(Client client, string message)
    {
        MessageToBroadCast mbc = JsonSerializer.Deserialize<MessageToBroadCast>(message)!;
        switch (mbc.MessageType)
        {
            case PackageMessageType.UserConnected:
            {
                client.UserName = mbc.Message!.ToString()!;
                MessageToBroadCast toBroadCast = new MessageToBroadCast()
                {
                    MessageType = PackageMessageType.UserConnected,
                    Message = client
                };
                foreach (Client chatClient in _chatClients)
                {
                    client.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new MessageToBroadCast
                    {
                        MessageType = PackageMessageType.UserConnected,
                        Message = chatClient
                    })));
                }
                foreach (Client chatClient in _chatClients)
                {
                    chatClient.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toBroadCast)));
                }
            }
                break;
            case PackageMessageType.UserDisconnected:
            {
                MessageToBroadCast toBroadCast = new MessageToBroadCast()
                {
                    MessageType = PackageMessageType.UserDisconnected,
                    Message = client
                };
                foreach (Client chatClient in _chatClients)
                {
                    chatClient.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toBroadCast)));
                }
            }
                break;
            case PackageMessageType.MessageSentToChat:
            {
                MessageToBroadCast toBroadCast = new MessageToBroadCast()
                {
                    MessageType = PackageMessageType.MessageSentToChat,
                    Message = mbc.Message
                };
                foreach (Client chatClient in _chatClients)
                {
                    chatClient.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toBroadCast)));
                }
            }
                break;
        }
    }
}