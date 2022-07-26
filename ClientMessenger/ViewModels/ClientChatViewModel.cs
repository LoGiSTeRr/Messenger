using ClientMessenger.Enums;
using ClientMessenger.Messages;
using ClientMessenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientMessenger.ViewModels;

public partial class ClientChatViewModel : ViewModel
{
    [ObservableProperty]
    private IUserManager _manager;

    public ClientChatViewModel()
    {
        _manager = App.Container.GetInstance<UserManager>();
    }
}