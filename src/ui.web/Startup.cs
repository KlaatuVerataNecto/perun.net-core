using System;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using Microsoft.Extensions.Caching.Memory;
//using Elmah.Io.AspNetCore;
using persistance.dapper.common;
using persistance.dapper.repository;
using ui.web.Config;
using infrastructure.user.services;
using FluentValidation.AspNetCore;
using FluentValidation.Attributes;
using FluentValidation;
using ui.web.Infrastructure.FluentValidators;
using System.Collections.Generic;

namespace ui.web
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Simple Injector.
            services.AddSingleton<IControllerActivator>(
                      new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.UseSimpleInjectorAspNetRequestScoping(container);

            // Add mini profiler.
            services.AddMiniProfiler();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
           
            // Add framework services.
            services.AddMvc(options =>
            {
                options.SslPort = 44361;
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // TODO: Get Fluent Validation working with custom validators on the client  
            //services.AddTransient<IUserRegistrationService, UserRegistrationService>();          
            //services
            //    .AddMvc()
            //    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache)
        {
            // Initalize Simple Injector
            InitializeContainer(app);
            container.Verify();

            // Logging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Own implementation
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = ConfigVariables.AuthSchemeName,
                LoginPath = new PathString("/Account/Unauthorized/"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });


            // Google Login
            var googleOptions = new GoogleOptions
            {
                ClientId = ConfigVariables.GoogleClientId,
                ClientSecret = ConfigVariables.GoogleClientSecret,
                AutomaticChallenge = true,
            };
            googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
            googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
            app.UseGoogleAuthentication(googleOptions);


            // Add Elmah.

            // Setup
            app.UseStaticFiles();
            app.UseSession();

            // miniprofiler 
            app.UseMiniProfiler(new MiniProfilerOptions
            {
                // Path to use for profiler URLs
                RouteBasePath = "~/profiler",

                // Control which SQL formatter to use
                SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter(),

                // Control storage
                Storage = new MemoryCacheStorage(cache, TimeSpan.FromMinutes(60)),

                // To control authorization, you can use the Func<HttpRequest, bool> options:
                //ResultsAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler,
                //ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler,

                // To control which requests are profiled, use the Func<HttpRequest, bool> option:
                //ShouldProfile = request => MyShouldThisBeProfiledFunction(request),

                // Profiles are stored under a user ID, function to get it:
                //UserIdProvider =  request => MyGetUserIdFunction(request),

                // Optionally swap out the entire profiler provider, if you want
                // The default handles async and works fine for almost all appliations
                //ProfilerProvider = new MyProfilerProvider(),
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Connection string:
            string connectionString = Configuration.GetConnectionString("MySQLDatabase");
            
            // Register Dapper.NET:
            container.Register<IDbConn>(() => new DbConn(connectionString));
           
            // Repositories for Dapper.NET:
            var repositoryAssembly = new[] { typeof(UserRepository).GetTypeInfo().Assembly };
            var repositoryTypes = container.GetTypesToRegister(typeof(DapperService<>), repositoryAssembly);

            foreach (Type implementationType in repositoryTypes)
            {
                Type serviceType = implementationType.GetInterfaces().Where(i => !i.GetTypeInfo().IsGenericType).Single();
                container.Register(serviceType, implementationType, Lifestyle.Transient);
            }

            // Register services for infrastructure.user:
            var userServicesAssembly = typeof(UserAuthentiactionService).GetTypeInfo().Assembly;

            var userRegistrations =
                from type in userServicesAssembly.GetExportedTypes()
                where type.Namespace.StartsWith("infrastructure.user.services")
                where type.GetInterfaces().Any()
                select new
                {
                    Service = type.GetInterfaces().Single(),
                    Implementation = type
                };

            foreach (var reg in userRegistrations)
            {
                container.Register(reg.Service, reg.Implementation, Lifestyle.Transient);
            }

            // Register Fluent Validations

            //var assemblies = new List<Assembly>();
            //assemblies.Add(typeof(Startup).GetTypeInfo().Assembly);
            //container.Register<IValidatorFactory, ApplicationValidatorFactory>(Lifestyle.Singleton);
            //container.Register(typeof(IValidator<>), assemblies);
            //container.RegisterConditional(typeof(IValidator<>), typeof(ValidateNothingDecorator<>), Lifestyle.Singleton, context => !context.Handled);

            // Register Simple Injector validation factory in FV
            //FluentValidationModelValidatorProvider.Configure(provider =>
            //{
            //    provider.ValidatorFactory = new ApplicationValidatorFactory(container);
            //    provider.AddImplicitRequiredValidator = false;
            //    provider.Add(typeof(UniqueEmailValidator), (metadata, context, description, validator) => new UniqueEmailPropertyValidator(metadata, context, description, validator));
            //    provider.Add(typeof(UniqueUsernameValidator), (metadata, context, description, validator) => new UniqueUsernamePropertyValidator(metadata, context, description, validator));
            //    provider.Add(typeof(StringNoSpacesValidator), (metadata, context, description, validator) => new StringNoSpacesPropertyValidator(metadata, context, description, validator));
            //    provider.Add(typeof(PasswordStrengthValidator), (metadata, context, description, validator) => new PasswordStrengthPropertyValidator(metadata, context, description, validator));
            //}
            //);
        }
    }
}
