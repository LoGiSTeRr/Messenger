using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatModelLibrary;
using ChatModelLibrary.Messages;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServerMessenger;

public class HostServer
{
    private Socket _server;
    private IPEndPoint _endPoint;
    private ConcurrentDictionary<Guid, Client> _chatClients;

    public HostServer()
    {
        _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ipAddress = IPAddress.Parse("127.0.0.1");
        _endPoint = new IPEndPoint(ipAddress, 44433);
        _chatClients = new ConcurrentDictionary<Guid, Client>();
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
                Client clientConnected = new Client(_server.Accept());
                var clientKey = Guid.NewGuid();
                _chatClients.TryAdd(clientKey, clientConnected);

                Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("Socket connected");

                    byte[] messageBuffer = new byte[2048];

                    try
                    {
                        while (clientConnected.ClientSocket.Connected)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                ct.ThrowIfCancellationRequested();
                            }

                            await clientConnected.ClientSocket.ReceiveAsync(messageBuffer, SocketFlags.None);
                            Console.WriteLine("Message Received");
                            ReceiveMessage(clientKey, clientConnected, Encoding.UTF8.GetString(messageBuffer));
                            Array.Clear(messageBuffer);

                        }
                    }
                    catch (Exception ex) when (ex is SocketException or ObjectDisposedException)
                    {

                    }
                    finally
                    {
                        Console.WriteLine("Socket disconnected");
                        if (!string.IsNullOrEmpty(clientConnected.UserName))
                        {
                            List<MessageToBroadCast> toBroadCastAllUsers = new List<MessageToBroadCast>();
                            toBroadCastAllUsers.Add(new MessageToBroadCast()
                            {
                                MessageType = PackageMessageType.UserDisconnected,
                                Message = clientConnected.UserName
                            });
                            _chatClients.Remove(clientKey, out clientConnected!);
                            foreach (var (_, chatClient) in _chatClients)
                            {
                                chatClient.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toBroadCastAllUsers)));
                            }
                        }
                        clientConnected.ClientSocket.Close();
                        clientConnected.ClientSocket.Dispose();
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }, ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    private void ReceiveMessage(Guid clientkKey, Client client, string message)
    {
        List<MessageToBroadCast> mbds =
            JsonSerializer.Deserialize<List<MessageToBroadCast>>(message.Substring(0, message.IndexOf('\0')))!;

        foreach (var mbc in mbds)
        {
            switch (mbc.MessageType)
            {
                case PackageMessageType.UserConnected:
                {
                    client.UserName = mbc.Message!.ToString()!;
                    List<MessageToBroadCast> toBroadCastAllUsers = new List<MessageToBroadCast>();
                    toBroadCastAllUsers.Add(new MessageToBroadCast()
                    {
                        MessageType = PackageMessageType.UserConnected,
                        Message = client.UserName
                    });
                    
                    List<MessageToBroadCast> toUniCastAllUsers = new List<MessageToBroadCast>();
                    foreach (var (_, chatClient) in _chatClients)
                    {
                        toUniCastAllUsers.Add(new MessageToBroadCast
                        {
                            MessageType = PackageMessageType.UserConnected,
                            Message = chatClient.UserName
                        });
                    }
                    client.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toUniCastAllUsers)));

                    foreach (var (key, chatClient) in _chatClients)
                    {
                        if (key == clientkKey)
                        {
                            continue;
                        }
                        chatClient.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toBroadCastAllUsers)));
                    }
                }
                    break;
                case PackageMessageType.MessageSentToChat:
                {
                    List<MessageToBroadCast> toBroadCastAllUsers = new List<MessageToBroadCast>();
                    toBroadCastAllUsers.Add(new MessageToBroadCast()
                    {
                        MessageType = PackageMessageType.MessageSentToChat,
                        Message = mbc.Message
                    });
                    foreach (var (_, chatClient) in _chatClients)
                    {
                        chatClient.ClientSocket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toBroadCastAllUsers)));
                    }
                }
                    break;
            }
        }
    }
}