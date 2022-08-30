namespace ChatModelLibrary.Messages;

public class MessageToBroadCast : IMessageToBroadCast
{
    public PackageMessageType MessageType { get; set; }
    public object? Message { get; set; }
}