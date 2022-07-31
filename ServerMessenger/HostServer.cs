using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class HostServer
{
    private Socket _server;
    private IPAddress _ipAddress;
    private IPEndPoint _endPoint;
    private ConcurrentBag<Socket> _clientsSockets;

    public HostServer()
    {
        _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        _ipAddress = IPAddress.Parse("127.0.0.1");
        _endPoint = new IPEndPoint(_ipAddress, 55555);
        _clientsSockets = new ConcurrentBag<Socket>();
    }

    public void Launch()
    {
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
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
                
                Socket clientSocket = _server.Accept();

                _clientsSockets.Add(clientSocket);

                Task.Factory.StartNew(() =>
                {
                    if (!_clientsSockets.TryTake(out Socket? client))
                    {
                        return;
                    }

                    Console.WriteLine("Client connected");

                    byte[] messageBuffer = new byte[2048];

                    try
                    {
                        while (true)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                ct.ThrowIfCancellationRequested();
                            }
                            client.Receive(messageBuffer);
                            string[] messageReceived = Encoding.UTF8.GetString(messageBuffer).Split('|');
                            SendMessage(messageReceived[0], messageReceived[1], messageReceived[2]);
                        }

                    }
                    catch (Exception ex) when (ex is SocketException or ObjectDisposedException)
                    {
                        Console.WriteLine("Client disconnected");
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }, ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }
    private void SendMessage(string surname, string name, string message)
    {
        foreach (var socket in _clientsSockets)
        {
            socket.Send(Encoding.UTF8.GetBytes($"{surname}|{name}|{message}"));
        } 
    }
}