using System.Collections.Generic;
using ChatModelLibrary;
using ChatModelLibrary.Messages;

namespace ClientMessenger.Services;

public interface IMessageManager
{
    public IList<Message> Messages { get; }
    void AddMessage(Message message);
}