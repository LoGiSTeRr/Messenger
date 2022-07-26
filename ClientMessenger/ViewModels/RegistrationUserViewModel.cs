﻿using System.Threading;
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

    [ObservableProperty] private string? _username;

    [ObservableProperty] private string? _errorInfo;

    private IClientListener _clientServer;

    public RegistrationUserViewModel()
    {
        _clientServer = App.Container.GetInstance<ClientListener>();
        _userManager = App.Container.GetInstance<UserManager>();
        var uiContext = SynchronizationContext.Current;

        _clientServer.UserConnected += message =>
        {
            uiContext?.Send(
                _ => _userManager.AddUser(new User()
                {
                    Username = message.Message!.ToString()!
                }), null);
        };
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

        WeakReferenceMessenger.Default.Send(new ChangeViewMessage(ViewModelEnum.ClientChatViewModel));
        await _clientServer.StartListeningAsync();
    }
}