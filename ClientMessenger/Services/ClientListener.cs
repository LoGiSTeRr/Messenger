using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ChatModelLibrary;
using ChatModelLibrary.Messages;
using ClientMessenger.Models;

namespace ClientMessenger.Services;

public class ClientListener : IClientListener
{
    private Socket _client;
    private IPAddress _ipAddress;
    private IPEndPoint _endPoint;
    private IUserManager _userManager;
    private IMessageManager _messageManager;
    private User _user;

    public ClientListener()
    {
        _userManager = App.Container.GetInstance<UserManager>();
        _messageManager = App.Container.GetInstance<MessageManager>();
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _ipAddress = IPAddress.Parse("127.0.0.1");
        _endPoint = new IPEndPoint(_ipAddress, 44433);
    }

    public async Task<bool> Connect(string username)
    {
        try
        {
            await _client.ConnectAsync(_endPoint);
            IMessageToBroadCast message = new MessageToBroadCast()
            {
                MessageType = PackageMessageType.UserConnected,
                Message = new string(username)
            };
            _user = new User { Username = username };
            _client.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
            return true;
        }
        catch (SocketException e)
        {
            return false;
        }
    }

    public bool SendMessage(string message)
    {
        if (!_client.Connected)
        {
            return false;
        }

        try
        {
            MessageToBroadCast messageToBroadCast = new MessageToBroadCast()
            {
                MessageType = PackageMessageType.MessageSentToChat,
                Message = new Message { Content = message, MessageBy = _user.Username }
            };
            _client.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageToBroadCast)));
            return true;
        }
        catch (SocketException e)
        {
            return false;
        }
    }

    public async Task StartListeningAsync()
    {
        if (!_client.Connected)
        {
            await _client.ConnectAsync(_endPoint);
        }

        await Task.Run(async () =>
        {
            var messageBuffer = new byte[2048];

            while (true)
            {
                await _client.ReceiveAsync(messageBuffer, SocketFlags.None);
                string message = Encoding.UTF8.GetString(messageBuffer);
                ReceiveMessage(message);
            }
        });
    }

    private void ReceiveMessage(string message)
    {
        MessageToBroadCast? mbd =
            JsonSerializer.Deserialize<MessageToBroadCast>(message.Substring(0, message.IndexOf('\0')))!;

        switch (mbd.MessageType)
        {
            case PackageMessageType.UserConnected:
            {
                _userManager.AddUser(new User()
                {
                    Username = mbd.Message!.ToString()!
                });
            }
                break;
            case PackageMessageType.UserDisconnected:
            {
                _userManager.RemoveUser(_userManager.Users.First(user => user.Username == mbd.Message!.ToString()!));
            }
                break;
            case PackageMessageType.MessageSentToChat:
            {
                IMessage msg = mbd.Message as Message;
                _messageManager.AddMessage(new Message()
                {
                    Content = msg.Content,
                    MessageBy = mbd.Message!.ToString()!
                });
            }
                break;
        }
    }
}