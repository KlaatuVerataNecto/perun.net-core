# perun.net-core
ASP.NET MVC Core 1.1 base project with custom user and social logins

Simple web application project template powered by ASP.NET Core 1.1 that features the following:

- Powered by ASP.NET Core 1.1.
- Runs on Linux (Ubuntu Server).
- Uses MySQL for database.
- Combines custom login system with social logins (Google, Facebook).
- Unobtrusive validation
- Remote validation
- Avatar image upload (in progress).
- Multilanguage support (in progress).
- Unit Tests (todo)

It features the following libraries:
- Entity Framework Core - ORM used to access database.
- Pomelo Entity Framework Core - makes Entity Framework Core work with MySQL
- Autofac - default dependency injection.
- Serilog - logging.
- MiniProfiler - profiling application and sql (in progress).

Tutorials and how-to: 
http://klaatuveratanecto.com/

The complete application will consist of:
- Post creation (title, image) by the user.
- Post vote up and down.
- Post tagging.
- Post comments.
- Post comment voting.
- Post "Facebook Like".
- Gamification - points for creation of Posts and receiving votes
