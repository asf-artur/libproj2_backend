using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleApp1;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication2.Middlewares;
using WebApplication2.Repositories;
using WebApplication2.Services;
using BookConverterService = WebApplication2.Services.BookConverterService;
using ILogger = NLog.ILogger;

namespace WebApplication2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<BookCopyRepository>();
            services.AddSingleton<BookEventRepository>();
            services.AddSingleton<BookInfoRepository>();
            services.AddSingleton<BookSearchResultRepository>();
            services.AddSingleton<NotificationsRepository>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserAccountRepository>();

            services.AddScoped<BookBorrowService>();
            services.AddScoped<BookConverterService>();
            services.AddScoped<BookEventService>();
            services.AddScoped<BookSearchService>();
            services.AddSingleton<NotificationService>();
            services.AddScoped<SeleniumGetSearchInfo>();

            services.AddSpaStaticFiles(options => options.RootPath = "Front/dist");
            services.AddControllersWithViews();
            services.AddMvcCore()
                .AddApiExplorer()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddSwaggerGen();
            services.AddCors(); // добавляем сервисы CORS

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // авторизация
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // options.LoginPath = new PathString("/user/NoLogin");
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var j = JsonConvert.SerializeObject("not");
                        context.Response.WriteAsync(j);
                        return Task.CompletedTask;
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  Microsoft.Extensions.Hosting.IApplicationLifetime applicationLifetime, ILogger<Startup> logger)
        {
            var file = File.Exists("./Images/4.jpg");
            // var file = File.Exists("Logs\\logs_06.06.2021 19_21_14.txt");
            // var file = File.Exists("firebase_key.json");
            logger.LogError($"FILE exists {file}");
            logger.LogError($"{Directory.GetCurrentDirectory()}");
            logger.LogError($"{env.ContentRootPath}");

            file = File.Exists($"{env.ContentRootPath}/Images/4.jpg");
            logger.LogError($"FILE exists {file}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            // подключаем CORS
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
            });

            // app.UseCookiePolicy();

            logger.LogCritical("LogCritical {0}");
            logger.LogDebug("LogDebug {0}");
            logger.LogError("LogError {0}");
            logger.LogInformation("LogInformation {0}");
            logger.LogWarning("LogWarning {0}");


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseForwardedHeaders();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            // var d = DateTime.Now;


            app.UseMiddleware<EndpointLoggerMiddleware>(logger);

            try
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    // endpoints.MapControllerRoute(
                    //     name: "default",
                    //     pattern: "{controller=Home}/{action=Index}/");
                });
            }
            catch (Exception e)
            {
                app.Run(async (c) =>
                    await c.Response.WriteAsync($"lol Controllers Exception\n{e.Message}"));
            }
            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Front";
            });
        }
    }
}