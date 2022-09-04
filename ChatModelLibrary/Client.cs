using System.Net.Sockets;

namespace ChatModelLibrary;

public class Client : IClient
{
    public Socket ClientSocket { get; }
    public string? UserName { get; set; }
    
    public Client(Socket socket)
    {
        ClientSocket = socket;
    }

}