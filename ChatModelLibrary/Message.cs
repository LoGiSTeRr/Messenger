namespace ChatModelLibrary;

public class Message : IMessage
{
    public IUser MessageBy { get; set; }
    public string Content { get; set; }
}