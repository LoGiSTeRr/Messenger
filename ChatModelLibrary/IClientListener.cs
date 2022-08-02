using System.Threading.Tasks;

namespace ChatModelLibrary;

public interface IClientListener
{
    Task<bool> Connect();
    bool SendMessage(string surname, string name, string message);
}