namespace ChatModelLibrary;

public interface IMessage
{
    public IUser MessageBy { get; set; }
    public string Content { get; set; }
}