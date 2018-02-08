# Perun.net-core
ASP.NET MVC Core 2.0 base project with custom user and social logins.

Demo: https://peruncore.com

Simple web application project template powered by ASP.NET Core 2.0 that at the moment features:
- Powered by ASP.NET Core 2.0.
- Runs on Linux (Ubuntu Server).
- Uses MySQL for database.
- Combines custom login system with social logins (Google, Facebook).
- Unobtrusive validation.
- Remote validation.
- CQS approach (Dapper for querying, EF Core for writting)

Done:
- User avatar upload.
- Creating Post (Title ++ Image).

Todo:
- Post vote up and down.
- Post tagging.
- Post comments.
- Post comment voting.
- Post "Facebook Like".
- User Profile with activity view.
- Gamification - points for creation of Posts and receiving votes
- Multilanguage support.
- Unit Tests.

It features the following libraries:
- Entity Framework Core - ORM used to access database.
- Pomelo Entity Framework Core - makes Entity Framework Core work with MySQL
- Autofac - default dependency injection.
- Serilog - logging.
- MiniProfiler - profiling application and sql (in progress).
