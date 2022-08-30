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
}