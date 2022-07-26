namespace ClientMessenger.Models;

public class User
{
    public string Name { get; set; }
    public string LastName { get; set; }

    public string Prefix
    {
        get => $"{LastName[0]}{Name[0]}";
        set { }
    }
}