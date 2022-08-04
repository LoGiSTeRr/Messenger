namespace ChatModelLibrary;

public interface IUser
{
    public IClientListener ClientListener { get; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Prefix { get; }
    public string FullName { get; }
}
//https://stackoverflow.com/questions/38679972/determine-type-during-json-deserialize
// public class MessageWrapper<T>
// {
//     public string MessageType { get { return typeof(T).FullName; } }
//     public T Message { get; set; }
// }
//
// var nameMessage = new MessageWrapper<Name>();
// nameMessage.Message = new Name {First="Bob", Last = "Smith"};
// var serialized = JsonConvert.SerializeObject(nameMessage);
//
// When deserializing, first deserialize the JSON as this type:
//
// public class MessageWrapper
// {
//     public string MessageType { get; set; }
//     public object Message { get; set; }
// }
//
// var deserialized = JsonConvert.DeserializeObject<MessageWrapper>(serialized);
// Extract the message type from the MessageType property.
//
//     var messageType = Type.GetType(deserialized.MessageType);
// Now that you know the type, you can deserialize the Message property.
//
//     var message = JsonConvert.DeserializeObject(
//         Convert.ToString(deserialized.Message), messageType);
// message is an object, but you can cast it as Name or whatever class it actually is.