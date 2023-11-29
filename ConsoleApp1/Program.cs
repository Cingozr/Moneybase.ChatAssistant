using ChatAssistant.Domain.Entities;
using ConsoleApp1;

using (var db = new ChatAssistantContext())
{ 
    var teama = new Team { TeamName = "a", Shift = ChatAssistant.Domain.Enums.Shift.Morning };
    var agentsT1 = new Agent { AgentName = "T1", Team = teama, Seniority = ChatAssistant.Domain.Enums.AgentSeniority.MidLevel };

    var teamb = new Team { TeamName = "b", Shift = ChatAssistant.Domain.Enums.Shift.Afternoon };
    var agentsT2 = new Agent { AgentName = "T2", Team = teamb, Seniority = ChatAssistant.Domain.Enums.AgentSeniority.TeamLead };

    db.Teams.AddRange(teama, teamb);
    db.Agents.AddRange(agentsT1, agentsT2);
    db.SaveChanges();
}