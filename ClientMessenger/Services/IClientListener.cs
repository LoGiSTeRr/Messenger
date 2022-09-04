using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatModelLibrary;
using ChatModelLibrary.Messages;

namespace ClientMessenger.Services;

public interface IClientListener
{
    Task<bool> Connect(string username);
    bool SendMessage(string message);
    public Task StartListeningAsync();
    
    public event Action<MessageToBroadCast>? UserConnected;
    public event Action<MessageToBroadCast>? UserDisconnected;
    public event Action<MessageToBroadCast>? MessageSentToChat;
}