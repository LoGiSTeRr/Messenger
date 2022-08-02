namespace ChatModelLibrary;

public interface IUser
{
    public IClientListener ClientListener { get; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Prefix { get; }
    public string FullName { get; }
}

public interface IMessage
{
    public IUser MessageBy { get; set; }
    public string Content { get; set; }
}