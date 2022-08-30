using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatModelLibrary;
using ClientMessenger.Enums;
using ClientMessenger.Messages;
using ClientMessenger.Models;
using ClientMessenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientMessenger.ViewModels;

public partial class RegistrationUserViewModel : ViewModel
{
    [ObservableProperty] private IUserManager _userManager;

    [ObservableProperty] private string _username;

    [ObservableProperty] private string _errorInfo;

    private IClientListener _clientServer;

    public RegistrationUserViewModel()
    {
        _clientServer = App.Container.GetInstance<ClientListener>();
        _userManager = App.Container.GetInstance<UserManager>();
    }

    [ICommand]
    async void RegisterUser()
    {
        if (string.IsNullOrEmpty(Username))
        {
            ErrorInfo = "Incorrect username entered";
            return;
        }

        if (!await _clientServer.Connect(Username))
        {
            ErrorInfo = "Connection failed. Try again later";
            return;
        }
        await _clientServer.StartListeningAsync();
        
        WeakReferenceMessenger.Default.Send(new ChangeViewMessage(ViewModelEnum.ClientChatViewModel));


    }
}