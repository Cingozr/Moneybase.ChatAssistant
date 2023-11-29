using ChatAssistant.Application.Extensions;
using ChatAssistant.Infrastructure.ApiClients.Endpoints.ChatSessions;
using ChatAssistant.Infrastructure.Extensions;
using ChatAssistant.Notification.Api.Hubs.Chat;
using LiveChatSupport.Infrastructure.Data.Dtos;
using MediatR;
using Refit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddCors();


#region Refit
var chatBaseApi = builder.Configuration.GetSection("Apis:BaseApiAddress").Value;
var ChatSessionEndpoint = builder.Configuration.GetSection("Apis:ChatSessionEndpoint").Value;
builder.Services
    .AddRefitClient<IChatSessionApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{chatBaseApi}{ChatSessionEndpoint}"));

#endregion

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.Configure<ChatConfiguration>(builder.Configuration.GetSection("ChatConfiguration"));

builder.Services.AddHostedService<ChatMessageBackgroundService>();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatNotificationHub>("chat-notifications");

app.Run();
