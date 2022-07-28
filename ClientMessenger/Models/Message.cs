namespace ClientMessenger.Models;

public class Message
{
    public User MessageBy { get; set; }
    public string Content { get; set; }
}