namespace ChatModelLibrary;

public class User : IUser
{
    public IClientListener ClientListener { get; } = new ClientListener();
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Prefix => $"{LastName[0]}{Name[0]}";
    public string FullName => $"{LastName} {Name}";
}