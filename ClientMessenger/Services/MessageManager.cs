using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChatModelLibrary;
using ChatModelLibrary.Messages;

namespace ClientMessenger.Services;

public class MessageManager : IMessageManager
{
    public ObservableCollection<IMessage> Messages { get; }

    public MessageManager()
    {
        Messages = new ObservableCollection<IMessage>();
    }
    
    public void AddMessage(IMessage message)
    {
        Messages.Add(message);
    }
}