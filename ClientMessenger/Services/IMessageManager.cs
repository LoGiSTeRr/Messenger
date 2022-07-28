using System.Collections.Generic;
using ClientMessenger.Models;

namespace ClientMessenger.Services;

public interface IMessageManager
{
    public IList<Message> Messages { get; }
    void AddMessage(Message message);
}