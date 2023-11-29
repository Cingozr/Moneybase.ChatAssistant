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
