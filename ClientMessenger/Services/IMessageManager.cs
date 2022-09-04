using System.Collections.ObjectModel;
using ChatModelLibrary.Messages;

namespace ClientMessenger.Services;

public interface IMessageManager
{
    public ObservableCollection<IMessage> Messages { get; }
    void AddMessage(IMessage message);
}