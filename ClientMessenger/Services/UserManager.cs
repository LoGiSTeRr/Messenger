using System.Collections.Generic;
using ClientMessenger.Models;

namespace ClientMessenger.Services;

class UserManager : IUserManager
{
    public IList<User> Users { get; }

    public UserManager()
    {
        Users = new List<User>();
    }
    
    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public void RemoveUser(User user)
    {
        Users.Remove(user);

    }
}