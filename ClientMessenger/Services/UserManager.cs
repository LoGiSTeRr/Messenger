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
    
    public void AddUser()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveUser()
    {
        throw new System.NotImplementedException();
    }
}