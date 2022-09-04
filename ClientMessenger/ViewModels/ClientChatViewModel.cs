using System.Linq;
using System.Text.Json;
using System.Threading;
using ChatModelLibrary.Messages;
using ClientMessenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientMessenger.ViewModels;

public partial class ClientChatViewModel : ViewModel
{
    [ObservableProperty] private IUserManager _userManager;
    [ObservableProperty] private IMessageManager _messageManager;

    private IClientListener _clientServer;

    public ClientChatViewModel()
    {
        _clientServer = App.Container.GetInstance<ClientListener>();
        _userManager = App.Container.GetInstance<UserManager>();
        _messageManager = App.Container.GetInstance<MessageManager>();
        var uiContext = SynchronizationContext.Current;
        
        _clientServer.UserDisconnected += message =>
        {
            uiContext?.Send(
                _ => _userManager.RemoveUser(_userManager.Users.First(user =>
                    user.Username == message.Message!.ToString()!)), null);
        };

        _clientServer.MessageSentToChat += message =>
        {
            IMessage? msg = JsonSerializer.Deserialize<Message>(message.Message!.ToString()!);
            uiContext?.Send(_ => _messageManager.AddMessage(new Message()
            {
                Content = msg!.Content,
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