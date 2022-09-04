namespace ChatModelLibrary.Messages;

public interface IMessage
{
    public string? MessageBy { get; set; }
    public string? Content { get; set; }
}