namespace MessageBus.Messages
{
    public class UserRecievedMessage
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
