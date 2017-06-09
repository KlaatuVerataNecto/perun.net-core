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
using ui.web.Config;
using infrastructure.user.services;
using persistance.ef.common;
using persistance.ef.repository;

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
            services.AddMiniProfiler()
                    .AddEntityFramework();
           
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache)
        {
            // Initalize Simple Injector
            InitializeContainer(app);
            container.Verify();

            // Logging
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

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
            //container.Register<IDbConn>(() => new DbConn(connectionString));

            //// Repositories for Dapper.NET:
            //var repositoryAssembly = new[] { typeof(UserRepository).GetTypeInfo().Assembly };
            //var repositoryTypes = container.GetTypesToRegister(typeof(DapperService), repositoryAssembly);

            //foreach (Type implementationType in repositoryTypes)
            //{
            //    //Type serviceType = implementationType.GetInterfaces().Where(i => !i.GetTypeInfo().IsGenericType).Single();
            //    Type serviceType = implementationType.GetInterfaces().Single();
            //    container.Register(serviceType, implementationType, Lifestyle.Transient);
            //}

            // Register EF
            container.Register<IEFContext>(() => new EFContext(connectionString), Lifestyle.Transient);

            var repositoryAssembly = new[] { typeof(UserRepository).GetTypeInfo().Assembly };
            var repositoryTypes = container.GetTypesToRegister(typeof(UserRepository), repositoryAssembly);
            foreach (Type implementationType in repositoryTypes)
            {                
                Type serviceType = implementationType.GetInterfaces().Single();
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
        }
    }
}
