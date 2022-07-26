using ClientMessenger.Enums;

namespace ClientMessenger.Messages;

public class ChangeViewMessage
{
    public ViewModelEnum Value { get; set; }

    public ChangeViewMessage(ViewModelEnum value)
    {
        Value = value;
    }
}