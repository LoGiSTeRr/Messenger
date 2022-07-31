using ClientMessenger.Enums;
using ClientMessenger.Messages;
using ClientMessenger.Models;
using ClientMessenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientMessenger.ViewModels;

public partial class ClientChatViewModel : ViewModel
{
    [ObservableProperty] private IUserManager _userManager;

    [ObservableProperty] private IMessageManager _messageManager;

    public User SelectedUser { get; set; }
    public ClientChatViewModel()
    {
        _userManager = App.Container.GetInstance<UserManager>();
        _messageManager = App.Container.GetInstance<MessageManager>();
    }

    [ICommand]
    void SendMessage(string message)
    {
        SelectedUser.ClientListener.SendMessage(SelectedUser.FullName, SelectedUser.Name, message);
    }
}