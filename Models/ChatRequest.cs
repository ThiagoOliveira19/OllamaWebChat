namespace OllamaWebChat.Models
{
    public class ChatRequest
    {
        public string? Message { get; set; }
        public string? Response { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
