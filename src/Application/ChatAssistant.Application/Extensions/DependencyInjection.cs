using ChatAssistant.Application.Behaviors;
using ChatAssistant.Application.Behaviour;
using ChatAssistant.Infrastructure.Data.Repositories.Chats;
using ChatAssistant.Infrastructure.Data.Repositories.Teams;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChatAssistant.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
             
            return services;
        }
    }
}
