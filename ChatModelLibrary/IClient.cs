using System.Net.Sockets;

namespace ChatModelLibrary;

public interface IClient
{
    
    public Socket ClientSocket { get; }
    public string? UserName { get; set; }
}