using System.Linq;
using System.Text.Json;
using System.Threading;
using ChatModelLibrary;
using ChatModelLibrary.Messages;
using ClientMessenger.Enums;
using ClientMessenger.Messages;
using ClientMessenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientMessenger.ViewModels;

public partial class ClientChatViewModel : ViewModel
{
    [ObservableProperty] private IUserManager _userManager;
    [ObservableProperty] private IMessageManager _messageManager;

    private IClientListener _clientServer;
    private SynchronizationContext _uiContext;
    public ClientChatViewModel()
    {
        _clientServer = App.Container.GetInstance<ClientListener>();
        _userManager = App.Container.GetInstance<UserManager>();
        _messageManager = App.Container.GetInstance<MessageManager>();
        _uiContext = SynchronizationContext.Current;
        
        _clientServer.UserDisconnected += message =>
        {
            _uiContext.Send(
                x => _userManager.RemoveUser(_userManager.Users.First(user =>
                    user.Username == message.Message!.ToString()!)), null);
        };

        _clientServer.MessageSentToChat += message =>
        {
            IMessage? msg = JsonSerializer.Deserialize<Message>(message.Message.ToString());
            _uiContext.Send(x => _messageManager.AddMessage(new Message()
            {
                Content = msg.Content,
                MessageBy = msg.MessageBy
            }), null);
        };
    }
    
    [ICommand]
    void SendMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }
        _clientServer.SendMessage(message);
    }
}