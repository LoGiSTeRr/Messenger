using System.Net.Sockets;

namespace ChatModelLibrary;

public interface IClient
{
    
    public Socket ClientSocket { get; }
    public Guid UID { get; }
    public string UserName { get; set; }
}