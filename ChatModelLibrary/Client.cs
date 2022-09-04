using System.Net.Sockets;
using System.Text.Json;
using ChatModelLibrary.Messages;

namespace ChatModelLibrary;

public class Client : IClient
{
    public Socket ClientSocket { get; }
    public string UserName { get; set; }
    
    public Client(Socket socket)
    {
        ClientSocket = socket;
    }

}