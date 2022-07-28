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


    public ClientChatViewModel()
    {
        _userManager = App.Container.GetInstance<UserManager>();
        _messageManager = App.Container.GetInstance<MessageManager>();
        _userManager.AddUser(new User() { Name = "Daniel", LastName = "Borodin" });
        _userManager.AddUser(new User() { Name = "Lowok", LastName = "Loxov" });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys1", MessageBy = _userManager.Users[0] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys2", MessageBy = _userManager.Users[1] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys3", MessageBy = _userManager.Users[0] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys4", MessageBy = _userManager.Users[1] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys5", MessageBy = _userManager.Users[0] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys6", MessageBy = _userManager.Users[1] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys7", MessageBy = _userManager.Users[0] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys8", MessageBy = _userManager.Users[1] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys9", MessageBy = _userManager.Users[0] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys0", MessageBy = _userManager.Users[1] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys1", MessageBy = _userManager.Users[0] });
        _messageManager.AddMessage(new Message() { Content = "Yoooo hello guys2", MessageBy = _userManager.Users[1] });
    }
}