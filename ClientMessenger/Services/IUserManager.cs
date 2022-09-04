using System.Collections.ObjectModel;
using ClientMessenger.Models;

namespace ClientMessenger.Services;

public interface IUserManager
{
    ObservableCollection<User> Users { get; }
    void AddUser(User client);
    void RemoveUser(User client);
}