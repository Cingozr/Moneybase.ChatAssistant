namespace ChatAssistant.Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int SentBy { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }

}
