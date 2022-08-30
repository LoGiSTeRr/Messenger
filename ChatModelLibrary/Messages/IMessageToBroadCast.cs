namespace ChatModelLibrary.Messages;

public interface IMessageToBroadCast
{
    public PackageMessageType MessageType { get; }
    public object Message { get; set; }
}


