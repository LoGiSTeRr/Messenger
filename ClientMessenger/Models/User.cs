using System;

namespace ClientMessenger.Models;

public class User
{
    public IClientListener ClientListener { get; set; } = new ClientListener();
    public string Name { get; set; }
    public string LastName { get; set; }

    public string Prefix => $"{LastName[0]}{Name[0]}";

    public string FullName => $"{LastName} {Name}";
}