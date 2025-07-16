# PLAN DESIGN:
## Detailed Plan for C# API with Clean Architecture

## Overview
This document outlines a comprehensive plan for developing a robust and scalable C# API using Clean Architecture principles. The API will leverage technologies such as MSSQL, Dapper, Redis, JWT for authentication, and Serilog for logging. The architecture will ensure maintainability, testability, and performance optimization.

## Project Structure
The solution will consist of three main projects:

### 1. Core (Domain Layer)
- **Entities**: 
  - User
  - Role
  - Permission
  - Menu
- **Interfaces**: 
  - `IRepository`
  - `IUnitOfWork`
  - `ICacheService`
  - `IAuthService`
- **DTOs and Domain Services**: Define data transfer objects and domain-specific services.

### 2. Infrastructure
- **Data Access**: Utilize Dapper for efficient data access with MSSQL.
- **Caching**: Implement Redis for performance optimization.
- **Logging**: Configure Serilog for structured and centralized logging.
- **Authentication**: Implement JWT for secure token handling.

### 3. API (Presentation Layer)
- **Web API**: Develop an ASP.NET Core Web API project.
- **Controllers**: Create controllers for:
  - Authentication
  - User Management
  - Role and Permission Management
- **Middleware**: Implement middleware for JWT authentication and authorization enforcement.
- **Dependency Injection**: Set up DI for loose coupling and enhanced testability.

## Database
- **Connection String**: Configure MSSQL database connection string with secure storage.
- **Dapper**: Use Dapper for efficient SQL query execution and object mapping.
- **Migrations**: Create tables for:
  - Users
  - Roles
  - Permissions
  - UserRoles
  - RolePermissions
  - Menus
- **Performance**: Ensure referential integrity and indexing for optimal performance.

## Caching
- **Redis Integration**: Integrate Redis for caching frequently accessed data (e.g., user permissions and menus).
- **Cache Invalidation**: Implement strategies to maintain data consistency.
- **Configuration**: Set up Redis connection and error handling.

## Authentication and Authorization
- **JWT Tokens**: Generate and validate JWT tokens with expiration and refresh token support.
- **User  Login**: Create an endpoint for user authentication and secure JWT issuance.
- **Authorization**: Implement role-based and permission-based authorization using policies.
- **Middleware**: Enforce authorization policies and handle unauthorized access gracefully.

## Logging
- **Serilog Setup**: Configure Serilog to log to console, file, and external sinks (e.g., Seq, ELK).
- **Structured Logging**: Log API requests, responses, errors, and significant events.
- **Correlation IDs**: Use correlation IDs for tracing requests across services.

## Additional Considerations
- **Configuration Management**: Use `appsettings.json` for configuration with environment-specific overrides.
- **Separation of Concerns**: Maintain clean separation of concerns and dependency injection throughout the solution.
- **Sample Data**: Provide seed data for roles, permissions, and default users for initial setup.
- **Testing**: Include comprehensive unit and integration tests for critical components.
- **Documentation**: Offer detailed instructions for setup, running, and testing the API.

## Follow-up Steps
1. Create the solution and projects with the appropriate folder structure.
2. Implement domain entities, DTOs, and interfaces in the Core project.
3. Set up infrastructure services including database access, caching, logging, and authentication.
4. Develop API controllers, middleware, and configure dependency injection.
5. Implement unit and integration tests to ensure code quality and reliability.
6. Thoroughly test authentication, authorization, caching, and logging functionalities.
7. Provide detailed instructions for running, testing, and deploying the API.

---

This plan ensures a robust, maintainable, and scalable API solution that adheres to best practices in Clean Architecture and modern .NET development. Let's build something great! 🚀

## Features
- .NET6
- Entity Framework Core – Code First
- Repository Pattern
- UnitOfWork Pattern
- CQRS Pattern
- Mediatr
- CQRS Pipeline Caching (Redis,InMemory configure from appsettings.json file),Validation(Fluent Validation),Logging (Request-Response)
- Response Wrappers
- Validation Filter For FluentValidation Errors
- Serilog
- Automapper
- Docker
- JWT Authentication,Refresh Token
- Complete User Management  (Register / Forgot Password / Confirmation Mail) Without Identity
- Role Based Authorization
- Database Seeding
- Custom Exception Handling Middleware
- RabbitMQ for Send Mail operations


## How To Start .Net API

For api, you must edit the appsettings.json file email settings eg.

For RabbitMQ, you need install Erlang Windows + RabbitMQServer Windows to run.

If you want to use PostgreSQL, please uncomment ConnectionString in appsetting.json and options.eNpgsql in 
```sh
.src\Infrastructure\Persistence\PersistenceRegiration.cs
```

When the project is up, the migrations run automatically

After a database will be created. 

Default Admin Account : 

```sh
Username : admin
Password : 123qwe
```


### Template used in this project: NET6 Clean Onion Architecture
