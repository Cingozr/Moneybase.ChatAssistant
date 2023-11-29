using ChatAssistant.Domain.Entities;
using ChatAssistant.Domain.Enums;
using ChatAssistant.Infrastructure.Data;
using ChatAssistant.Infrastructure.Messaging.Services.RabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RabbitMQ.Client;
using System.Reflection;

namespace ChatAssistant.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatAssistantDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LiveChatSupportDb"));
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton(sp =>
            {
                var connectionFactory = new ConnectionFactory()
                {
                    Uri = new Uri(configuration.GetConnectionString("RabbitMQ"))
                };
                return connectionFactory.CreateConnection();
            });
            services.AddSingleton(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateModel();
            });
            services.AddSingleton<IRabbitMQClientService, RabbitMQClientService>();

           

            return services;
        }

        public static IApplicationBuilder AddInfrastructureDataSeed(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ChatAssistantDbContext>();

            context.Database.EnsureCreated();

            if (!context.Teams.Any())
            {
                var teamA = new Team
                {
                    TeamName = "Team A",
                    Shift = Shift.Morning,
                };

                var teamB = new Team
                {
                    TeamName = "Team B",
                    Shift = Shift.Afternoon,
                };

                var teamC = new Team
                {
                    TeamName = "Team C",
                    Shift = Shift.Night,
                };

                var teamOverflow = new Team
                {
                    TeamName = "Overflow",
                    Shift = Shift.None,
                };

                context.Teams.AddRange(teamA, teamB, teamC, teamOverflow);

                var agentsTeamA = new List<Agent>
                    {
                        new Agent { AgentName="Morpheus", Seniority = AgentSeniority.TeamLead, Team= teamA },
                        new Agent { AgentName="Neo", Seniority = AgentSeniority.MidLevel, Team= teamA },
                        new Agent { AgentName="Trinity", Seniority = AgentSeniority.MidLevel, Team= teamA },
                        new Agent { AgentName="Dozer", Seniority = AgentSeniority.Junior , Team= teamA}
                    };

                var agentsTeamB = new List<Agent>
                    {
                        new Agent { AgentName="Tank", Seniority = AgentSeniority.Senior, Team= teamB },
                        new Agent { AgentName="Kaufman", Seniority = AgentSeniority.MidLevel, Team= teamB },
                        new Agent { AgentName="Smith", Seniority = AgentSeniority.Junior, Team= teamB },
                        new Agent { AgentName="Cypher", Seniority = AgentSeniority.Junior, Team= teamB }
                    };

                var agentsTeamC = new List<Agent>
                    {
                        new Agent { AgentName="Switch", Seniority = AgentSeniority.MidLevel, Team= teamC },
                        new Agent { AgentName="Mouse", Seniority = AgentSeniority.MidLevel, Team= teamC }
                    };

                var agentsOverflow = Enumerable.Range(1, 6).Select(i => new Agent
                {
                    AgentName = $"Ajan Smith Clone {i}",
                    Seniority = AgentSeniority.Junior,
                    Team = teamOverflow
                }).ToList();

                context.Agents.AddRange(agentsTeamA);
                context.Agents.AddRange(agentsTeamB);
                context.Agents.AddRange(agentsTeamC);
                context.Agents.AddRange(agentsOverflow);

                context.SaveChanges();

            }
            return app;
        }
    }
}
