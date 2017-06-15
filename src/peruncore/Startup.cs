﻿using System;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Autofac;
using Autofac.Extensions.DependencyInjection;

using peruncore.Config;
using infrastructure.user.services;
using persistance.ef.common;
using persistance.ef.repository;


namespace peruncore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                              .ReadFrom.Configuration(builder)
                              .CreateLogger();

            Configuration = builder;
        }

        public IContainer ApplicationContainer { get; private set; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.  
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add mini profiler.
            services.AddMiniProfiler()
                    .AddEntityFramework();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            // Add framework services.
            services.AddMvc();
            //services.AddMvc(options =>
            //{
            //    options.SslPort = 44361;
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});

            // Add Configuration 
            services.AddOptions();
            services.Configure<AuthSchemeSettings>(Configuration.GetSection("AuthSchemeSettings"));
            services.Configure<ImageUploadSettings>(Configuration.GetSection("ImageUploadSettings"));

            // Data protection 
            services.AddDataProtection();

            // DI
            string connectionString = Configuration.GetConnectionString("MySQLDatabase");
           
            // Create the container builder.
            var builder = new ContainerBuilder();
            // Register EF       
            builder.RegisterInstance(new EFContext(connectionString)).As<IEFContext>();
            
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


            builder.Populate(services);
            var container = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache, IApplicationLifetime appLifetime)
        {
            // Logging
            loggerFactory.AddSerilog();
            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            // error handling 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // read config 
            var authSchemeSettings = Configuration.GetSection("AuthSchemeSettings:Default");

            // DI
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());

            // Own implementation
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = Configuration.GetSection("AuthSchemeSettings:Default").Value,
                LoginPath = new PathString("/Account/Unauthorized/"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });


            // Google Login
            var googleOptions = new GoogleOptions
            {
                ClientId = Configuration.GetSection("SocialLoginSettings:GoogleClientId").Value ,
                ClientSecret = Configuration.GetSection("SocialLoginSettings:GoogleClientSecret").Value,
                AutomaticChallenge = true,
            };
            googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
            googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
            app.UseGoogleAuthentication(googleOptions);

            // status code page

            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "Status code page, status code: " +
                    context.HttpContext.Response.StatusCode);
            });

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
    }
}