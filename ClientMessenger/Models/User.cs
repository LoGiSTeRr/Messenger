using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientMessenger.Models;

public class User
{
    public ClientListener ClientListener { get; set; } = new ClientListener();
    public string Name { get; set; }
    public string LastName { get; set; }

    public string Prefix
    {
        get => $"{LastName[0]}{Name[0]}";
        set { }
    }

    public string FullName
    {
        get => $"{LastName} {Name}";
        set { }
    }
}

public class ClientListener
{
    private Socket _client;
    private IPAddress _ipAddress;
    private IPEndPoint _endPoint;

    public ClientListener()
    {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        _ipAddress = IPAddress.Parse("127.0.0.1");
        _endPoint = new IPEndPoint(_ipAddress, 44433);
    }

    public bool Connect()
    {
        try
        {
            _client.Connect(_endPoint);
            return true;
        }
        catch (SocketException e)
        {
            return false;
        }  
    }

    public bool SendMessage(string surname, string name, string message)
    {
        try
        {
            _client.Send(Encoding.UTF8.GetBytes($"{surname}|{name}|{message}"));
            return true;
        }
        catch (SocketException e)
        {
            return false;
        }
    }

    private void StartListening()
    {
        Task.Factory.StartNew(() =>
        {
            byte[] messageBuffer = new byte[2048];
            
            _client.Bind(_endPoint);
            _client.Listen();
            while (true)
            {
                _client.Receive(messageBuffer);
                string message = Encoding.UTF8.GetString(messageBuffer);
            }
        }, TaskCreationOptions.LongRunning);
    }
    
}