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




# Used Technologies
This section provides brief descriptions of the technologies used in the project.
## MediatR
MediatR is a mediator library for .NET. It separates commands, queries, and other logical operations in your application, reducing inter-class dependencies for cleaner and more manageable code.

## RabbitMQ
RabbitMQ is an open-source message broker software. It facilitates asynchronous communication between applications, enhancing scalability and flexibility in high-traffic scenarios and large data flows.

## Refit
Refit is a REST API client library for .NET. It's used for managing HTTP requests, allowing easy creation, management, and testing of your API calls.

## FluentValidation
FluentValidation is a powerful validation library for .NET. It allows you to define object validation rules in a clear, understandable, and maintainable way.

## AutoMapper
AutoMapper is an object-to-object mapping tool for .NET. It simplifies data transfer between different objects, reducing the need for manual mapping and improving code readability and maintenance.

