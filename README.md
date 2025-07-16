\# NET6 Clean Onion Architecture Project

\## Features + Plan Customize
\- .NET6
\- Entity Framework Core – Code First
\- Repository Pattern
\- UnitOfWork Pattern
\- CQRS Pattern
\- Mediatr
\- CQRS Pipeline Caching (Redis,InMemory nfigure from appsettings.json file),Validationluent Validation),Logging (Request-Response)
\- Response Wrappers
\- Validation Filter For FluentValidation Errors
\- Serilog
\- Automapper
\- Docker
\- JWT Authentication,Refresh Token
\- Complete User Management  (Register / Forgot ssword / Confirmation Mail) Without Identity
\- Role Based Authorization
\- Database Seeding
\- Custom Exception Handling Middleware
\- RabbitMQ for Send Mail operations


\## How To Start .Net API

For api, you must edit the appsettings.json file ail settings eg.

For RabbitMQ Install Erlang Windows + bbitMQServer Windows to run.
If you want to use PostgreSQL, please uncomment nnectionString in appsetting.json and options.eNpgsql in .src\\Infrastructure\\Persistence\\PersistenceRegiration.cs
When the project is up, the migrations run tomatically

After a database will be created. 

Default Admin Account : 

```sh
Username : admin
Password : 123qwe
```



