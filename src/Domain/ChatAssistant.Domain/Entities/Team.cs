using ChatAssistant.Domain.Enums;

namespace ChatAssistant.Domain.Entities
{
    public class Team
    {
        public Team()
        {
            Agents = new List<Agent>();
        }
        public int Id { get; set; }
        public string TeamName { get; set; }
        public Shift Shift { get; set; }
        public virtual ICollection<Agent> Agents { get; set; }
    }

}
