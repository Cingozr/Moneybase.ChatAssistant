using ChatAssistant.Domain.Enums;

namespace ChatAssistant.Domain.Entities
{
    public class Agent
    {
        public int Id { get; set; }
        public string AgentName { get; set; }
        public AgentSeniority Seniority { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public double Efficiency
        {
            get
            {
                return Seniority switch
                {
                    AgentSeniority.Junior => 0.4,
                    AgentSeniority.MidLevel => 0.6,
                    AgentSeniority.Senior => 0.8,
                    AgentSeniority.TeamLead => 0.5,
                    _ => 0,
                };
            }
        }
    }

}
