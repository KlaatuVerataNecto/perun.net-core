using Autofac;
using Autofac.Extensions.DependencyInjection;
using infrastructure.email.interfaces;
using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using peruncore.Config;
using peruncore.Infrastructure.Auth;
using peruncore.Infrastructure.Middleware;
using System;

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
            services.AddMvc(options =>
            {
                //options.SslPort = 44362;
                //options.Filters.Add(new RequireHttpsAttribute());
            });

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

            //services.AddAuthorization(o =>
            //{
            //    o.AddPolicy("Users", p => p.RequireClaim("Users"));
            //    o.AddPolicy("SuperUsers", p => p.RequireClaim("SuperUsers"));
            //    o.AddPolicy("PlatformUsers", p => p.RequireClaim("PlatformUsers"));
            //});


            // Cookies.
            services.AddAuthentication(Configuration.GetSection("AuthSchemeSettings:Application").Value)
                .AddCookie(
                Configuration.GetSection("AuthSchemeSettings:Application").Value, 
                options =>
                {
                    //AutomaticAuthenticate = true,
                    options.LoginPath = new PathString("/Account/Unauthorized/");
                    options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                    options.ExpireTimeSpan = TimeSpan.FromDays(int.Parse(Configuration.GetSection("AuthSchemeSettings:ExpiryDays").Value));
                }
            );

            services.AddAuthentication(Configuration.GetSection("AuthSchemeSettings:Google").Value)
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetSection("SocialLoginSettings:GoogleClientId").Value;
                    options.ClientSecret = Configuration.GetSection("SocialLoginSettings:GoogleClientSecret").Value;
                    options.SignInScheme = Configuration.GetSection("AuthSchemeSettings:Application").Value;
                    options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
                    options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
                }
            );

            services.AddAuthentication(Configuration.GetSection("AuthSchemeSettings:Facebook").Value)
                .AddFacebook(options =>
                {
                    options.ClientId = Configuration.GetSection("SocialLoginSettings:FacebookClientId").Value;
                    options.ClientSecret = Configuration.GetSection("SocialLoginSettings:FacebookClientSecret").Value;
                    options.SignInScheme = Configuration.GetSection("AuthSchemeSettings:Application").Value;
                }
            );


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

            // miniprofiler 
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
