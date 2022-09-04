namespace ChatModelLibrary.Messages;

public class Message : IMessage
{
    public string? MessageBy { get; set; }
    public string? Content { get; set; }
}