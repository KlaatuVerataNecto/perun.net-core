﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;

using infrastructure.email.interfaces;
using infrastructure.user.interfaces;
using infrastucture.libs.image;
using peruncore.Config;
using peruncore.Infrastructure.Auth;

namespace peruncore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // That's IIS Express specific
            /*services.AddMvc(options =>
            {
                options.SslPort = 44361;
                options.Filters.Add(new RequireHttpsAttribute());
            });*/

            services.AddMemoryCache();
            services.AddSession();
            services.AddDataProtection();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            // Add Configuration.
            services.AddOptions();
            services.Configure<AuthSchemeSettings>(Configuration.GetSection("AuthSchemeSettings"));
            services.Configure<ImageUploadSettings>(Configuration.GetSection("ImageUploadSettings"));
            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            // Register Email and Auth Settings.
            services.AddSingleton<IEmailSettingsService, EmailSettingsService>();
            services.AddSingleton<IAuthSchemeNameService, AuthSchemeNameService>();

            // Add mini profiler.
            services.AddMiniProfiler().AddEntityFramework();
            
            // Image service
            services.AddSingleton<IImageService, DefaultImageService>();

            //services.AddAuthorization(o =>
            //{
            //    o.AddPolicy("Users", p => p.RequireClaim("Users"));
            //    o.AddPolicy("SuperUsers", p => p.RequireClaim("SuperUsers"));
            //    o.AddPolicy("PlatformUsers", p => p.RequireClaim("PlatformUsers"));
            //});

            // Cookies.
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = Configuration.GetSection("AuthSchemeSettings:Application").Value;
                options.DefaultAuthenticateScheme = Configuration.GetSection("AuthSchemeSettings:Application").Value;
                options.DefaultScheme = Configuration.GetSection("AuthSchemeSettings:Application").Value;
            })
            .AddCookie(
                Configuration.GetSection("AuthSchemeSettings:Application").Value,
                options =>
                {
                    options.Cookie.HttpOnly = true;
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/login";
                    options.LogoutPath = "/user/logout";
                    options.AccessDeniedPath = "/error/accessdenied";
                    options.SlidingExpiration = true;
                    options.ReturnUrlParameter = "return_url";
                }
            )
            .AddGoogle(options =>
             { 
                 options.ClientId = Configuration.GetSection("SocialLoginSettings:GoogleClientId").Value;
                 options.ClientSecret = Configuration.GetSection("SocialLoginSettings:GoogleClientSecret").Value;
                 options.SignInScheme = Configuration.GetSection("AuthSchemeSettings:Application").Value;
                 options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
                 options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");

             })
            .AddFacebook(options =>
            {
                options.ClientId = Configuration.GetSection("SocialLoginSettings:FacebookClientId").Value;
                options.ClientSecret = Configuration.GetSection("SocialLoginSettings:FacebookClientSecret").Value;
                options.SignInScheme = Configuration.GetSection("AuthSchemeSettings:Application").Value;
            });

            // DI Config.
            string connectionString = Configuration.GetConnectionString("MySQLDatabase");
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new DependencyInjectionModule() { ConnectionString = connectionString});
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseMiddleware<RemoteIpAddressLoggingMiddleware>();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMiniProfiler();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
