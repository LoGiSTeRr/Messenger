using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    [ObservableProperty] private string _name;
    [ObservableProperty] private string _lastName;

    [ObservableProperty] private string _errorInfo;

    public RegistrationUserViewModel()
    {
        _userManager = App.Container.GetInstance<UserManager>();
    }

    [ICommand]
    async void RegisterUser()
    {
        if (_userManager.Users.FirstOrDefault(u => u.FullName == $"{LastName} {Name}") is not null)
        {
            ErrorInfo = "The user with this fullname already exists";
            return;
        }
        if (string.IsNullOrEmpty(Name))
        {
            ErrorInfo = "Incorrect name entered";
            return;
        }
        if (string.IsNullOrEmpty(LastName))
        {
            ErrorInfo = "Incorrect lastname entered";
            return;
        }

        User user = new User()
        {
            Name = Name,
            LastName = LastName
        };

        if (!await user.ClientListener.Connect())
        {
            ErrorInfo = "Connection failed. Try again later";
            return;
        }
        
        WeakReferenceMessenger.Default.Send(new ChangeViewMessage(ViewModelEnum.ClientChatViewModel));
        _userManager.AddUser(user);
        App.Container.GetInstance<ClientChatViewModel>().SelectedUser = user;
        

    }
}