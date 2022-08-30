using System.Collections.Generic;
using ChatModelLibrary;
using ChatModelLibrary.Messages;

namespace ClientMessenger.Services;

public class MessageManager : IMessageManager
{
    public IList<Message> Messages { get; }

    public MessageManager()
    {
        Messages = new List<Message>();
    }
    
    public void AddMessage(Message message)
    {
        Messages.Add(message);
    }
}