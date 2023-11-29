# Moneybase.ChatAssistant


![image](https://github.com/Cingozr/Moneybase.ChatAssistant/assets/14148180/6bd380cb-ad08-451b-adba-0f519f73903f)

 

# Project Structure
This document outlines the architectural structure of the ChatAssistant project.

## Api
The `Api` folder contains the API layer of the project. This layer includes services and interfaces that interact with the external world.
- **ChatAssistant.Notification.Api**: Manages API operations related to notification systems.
- **ChatAssistant.Support.Api**: Manages API operations for support and customer services.

## Application
The `Application` folder encompasses the business logic of the application. This layer defines the core functionalities and workflows of the project.
- **ChatAssistant.Application**: Contains the central business logic and workflows of the application.

## Domain
The `Domain` folder houses the domain model and business rules of the project. This layer outlines the fundamental objects of the application and how they interact.
- **ChatAssistant.Domain**: Includes the fundamental domain models and business rules of the application.

## Infrastructure
The `Infrastructure` folder addresses the infrastructure needs of the project. It includes database access, file system interactions, and other external services.
- **ChatAssistant.Infrastructure**: Contains components that meet the infrastructure needs of the application.

## Presentation
The `Presentation` folder is responsible for the user interface and user experience. This layer is where the application interacts with the end-user.
- **ChatAssistant.Client**: Manages components related to the user interface and user experience.




## Automatic Database Operations at Project Start
When the project is first launched, Teams and Agents, Team working hours type, and the teams that the agents belong to are automatically added to the database.

## Project Execution Instructions
To start the project, you should run the following three projects simultaneously:

- `ChatAssistant.Notification.Api`
- `ChatAssistant.Support.Api`
- `ChatAssistant.Client`

### ChatAssistant.Client User Interface
When you open `ChatAssistant.Client`, the following screen will appear on the homepage.

![Screenshot_5](https://github.com/Cingozr/Moneybase.ChatAssistant/assets/14148180/735e468f-e57b-4c4d-823e-03a0f242365d)


## Support Request Process
When you submit a "Request a chat support," the system receives your request and adds it to the queue. Then, `ChatMessageBackgroundService`, a background service running in `ChatAssistant.Notification.Api`, takes the messages from the queue one by one and distributes them according to the defined business rules.

### Assignment Rules

- Chats are assigned in a cyclical order, starting from junior representatives to mid-level and then to senior representatives.

### Support Request Assignment Rules

- Representatives work in three 8-hour shifts:
  ```csharp
  public enum Shift
  {
      None = 0,
      Morning = 1,
      Afternoon = 2,
      Night = 3,
  }
  ```
- At the end of each shift, assignments are made to the new shift team. The previous shift team continues their current chats and completes the process themselves.
- Capacity is calculated by multiplying the number of available representatives by their seniority levels and using the round-down method.
- The maximum allowed queue length is set at 1.5 times the team capacity.
- When the maximum queue length is reached and it's during office hours, an "overflow" team consisting of extra personnel who normally don't do this work is activated.
- The maximum concurrency (the number of chats each representative can handle simultaneously) is considered 10. Note: I updated this value to 2 during the working process. To update the value, you can adjust the `MaxConcurrentChatsPerAgent` value in `appsettings.json` in the `ChatAssistant.Notification.Api` project according to your test.
- Efficiency is multiplied by the following ratios based on seniority levels:
  - Junior: 0.4
  - Mid-Level: 0.6
  - Senior: 0.8
  - Team Lead: 0.5

AssignChatSessionCommandHandler.cs(ChatAssistant.Application.Commands.AssignChatSession) This is the code block where chat assignment is made.
 
##The description below is the description of the functions in this code block.


# AssignChatSessionCommandHandler Functions

## Constructor: `AssignChatSessionCommandHandler`
Initializes the command handler with necessary repositories and services, including team and chat session repositories, RabbitMQ client service, and chat configuration.

## Handle: `Handle`
Handles the `AssignChatSessionCommand` request by processing the chat session assignment based on various business rules and conditions.

## DeserializeChatSession: `DeserializeChatSession`
Converts a JSON string message into a `ChatSession` object.

## IsChatSessionActive: `IsChatSessionActive`
Checks if a given chat session is currently active by querying the chat session repository.

## GetActiveTeam: `GetActiveTeam`
Retrieves the active team based on the current shift timing from the team repository.

## AssignAgent: `AssignAgent`
Assigns an agent to a chat session from the active team or the overflow team if necessary, based on availability and business rules.

## CreateBadRequestResponse: `CreateBadRequestResponse`
Creates a response model indicating a bad request (HTTP 400) status.

## CreateSuccessResponse: `CreateSuccessResponse`
Creates a response model indicating a successful operation (HTTP 200) with the chat session data.

## CreateServiceUnavailableResponse: `CreateServiceUnavailableResponse`
Creates a response model indicating service unavailability (HTTP 503) with the chat session data.

## IsShiftActive: `IsShiftActive`
Determines if a given team's shift is currently active based on the current time.

## GetAvailableAgent: `GetAvailableAgent`
Identifies an available agent from a team, considering their availability and the number of active chat sessions.

## AssignSessionToAgentAsync: `AssignSessionToAgentAsync`
Assigns a chat session to a specified agent and updates the chat session details in the repository.

## IsAgentAvailable: `IsAgentAvailable`
Checks if an agent is available to take on a new chat session based on their current active chat sessions and maximum concurrent chat limit.

## IsQueueFull: `IsQueueFull`
Determines if the chat session queue has reached its maximum capacity based on the active team's capacity and a defined multiplier.

## GetQueueLength: `GetQueueLength`
Retrieves the current length of the chat session queue from the RabbitMQ client service.

## GetOverflowTeam: `GetOverflowTeam`
Selects the overflow team from the list of teams, typically used when the regular team's capacity is exceeded.

## ShiftTimings: `ShiftTimings`
A nested class defining constants for shift start and end times, as well as the overflow capacity multiplier.


