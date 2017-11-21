using Autofac;
using AutoMapper;
using System.Collections.Generic;
using System.Reflection;

using command.handlers.post;
using query.handlers.post;
using infrastructure.cqs;
using infrastructure.email.interfaces;
using infrastructure.email.services;
using infrastructure.user.services;
using infrastucture.libs.providers;
using persistance.dapper.common;
using persistance.ef.common;
using persistance.ef.repository;
using peruncore.Infrastructure.Auth;
using infrastructure.user.interfaces;

namespace peruncore.Config
{
    public class DependencyInjectionModule : Autofac.Module
    {
        public string ConnectionString { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ConnectionStringProvider { ConnectionString = ConnectionString })
                .As<IConnectionStringProvider>()
                .AsSelf();

            // Register Dapper
            builder.RegisterType<DapperConnectionFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DapperConnection>().AsSelf();

            // Register EF 
            builder.RegisterType<EFContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<EFContext>().AsSelf();

            // Register Generic Repository
            builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>));

            // Register Repositories
            var repositoryAssembly = typeof(UserRepository).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(repositoryAssembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces();

            // Register User Services 
            var userInfrastructureAssembly = typeof(UserAuthentiactionService).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(userInfrastructureAssembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();

            // Register User Services 
            var emailInfrastructureAssembly = typeof(EmailService).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(emailInfrastructureAssembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();

            // Add Automapper
            builder.RegisterAssemblyTypes(userInfrastructureAssembly).AssignableTo(typeof(Profile)).As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                    cfg.AddProfile(profile);

            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();


            // Query Handlers 
            var queryHandlers = typeof(GetAllPublishedPostsQueryHandler).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(queryHandlers).AsImplementedInterfaces();

            // Register Commmand Dispatcher 
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

            // Command Handlers 
            var commandHandlers = typeof(PostCommandHandlers).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(commandHandlers).AsImplementedInterfaces();

            // Settings
            builder.RegisterType<EmailSettingsService>().As<IEmailSettingsService>().SingleInstance();
            builder.RegisterType<AuthSchemeNameService>().As<IAuthSchemeNameService>().SingleInstance();
        }
    }
}
