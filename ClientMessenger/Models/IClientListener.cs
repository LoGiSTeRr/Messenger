using System.Threading.Tasks;

namespace ClientMessenger.Models;

public interface IClientListener
{
    Task<bool> Connect();
    bool SendMessage(string surname, string name, string message);
}