namespace ChatAssistant.Domain.Entities
{
    public class ChatSession
    {

        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public int AgentId { get; set; } = 0;
        public DateTime QueueTime { get; set; }
        public DateTime? LastPollTime { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsActive { get; set; }
         
    }

}
