using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClientMessenger.Services;

namespace ClientMessenger.Models;

public class ClientListener : IClientListener
{
    private Socket _client;
    private IPAddress _ipAddress;
    private IPEndPoint _endPoint;

    private bool isConnected = false;


    public ClientListener()
    {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        _ipAddress = IPAddress.Parse("127.0.0.1");
        _endPoint = new IPEndPoint(_ipAddress, 44433);
    }

    public async Task<bool> Connect()
    {
        try
        {
            _client.Connect(_endPoint);
            isConnected = true;
            await StartListeningAsync();
            return true;
        }
        catch (SocketException e)
        {
            return false;
        }
    }

    public bool SendMessage(string surname, string name, string message)
    {
        if (!isConnected)
        {
            return false;
        }

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

    private async Task StartListeningAsync()
    {
        await Task.Run(() =>
        {
            if (!isConnected)
            {
                return;
            }
            MessageBox.Show("1");

            Task.Factory.StartNew(() =>
            {
                var messageBuffer = new byte[2048];

                _client.Bind(_endPoint);
                _client.Listen();
                MessageBox.Show("2");
                while (true)
                {
                    _client.Receive(messageBuffer);
                    MessageBox.Show("3");
                    string message = Encoding.UTF8.GetString(messageBuffer);
                    ReceiveMessage(message);
                }
            }, TaskCreationOptions.LongRunning);
        });
    }

    private void ReceiveMessage(string message)
    {
        string[] messageReceived = message.Split('|');
        App.Container.GetInstance<MessageManager>().AddMessage(new Message()
        {
            Content = messageReceived[2],
            MessageBy = App.Container.GetInstance<UserManager>().Users
                .First(u => u.FullName == $"{messageReceived[0]} {messageReceived[1]}")
        });
    }
}