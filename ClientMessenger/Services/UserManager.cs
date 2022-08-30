using System.Collections.ObjectModel;
using ClientMessenger.Models;

namespace ClientMessenger.Services;

class UserManager : IUserManager
{
    public ObservableCollection<User> Users { get; }

    public UserManager()
    {
        Users = new ObservableCollection<User>();
    }
    
    public void AddUser(User client)
    {
        Users.Add(client);
    }

    public void RemoveUser(User client)
    {
        Users.Remove(client);
    }
}