﻿using System.Collections.Generic;
using ClientMessenger.Models;

namespace ClientMessenger.Services;

public interface IUserManager
{
    IList<User> Users { get; }
    void AddUser();
    void RemoveUser();
}