namespace KafkaChat.Shared
{
    public class ChatMessage
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageId { get; set; }

        public ChatMessage()
        {
            Timestamp = DateTime.Now;
            MessageId = Guid.NewGuid().ToString();
        }

        public ChatMessage(string username, string message) : this()
        {
            Username = username;
            Message = message;
        }

        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {Username}: {Message}";
        }

        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static ChatMessage FromJson(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<ChatMessage>(json);
        }
    }
}
