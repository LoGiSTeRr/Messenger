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
    public ClientChatViewModel()
    {
        _clientServer = App.Container.GetInstance<ClientListener>();
        _userManager = App.Container.GetInstance<UserManager>();
        _messageManager = App.Container.GetInstance<MessageManager>();
    }
    
    [ICommand]
    void SendMessage(string message)
    {
        _clientServer.SendMessage(message);
    }
}