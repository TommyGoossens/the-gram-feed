using System.Reflection;
using AspNetCore.Firebase.Authentication.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheGramFeed.EventBus;
using TheGramFeed.EventBus.Connection;
using TheGramFeed.Helpers;
using TheGramFeed.Repository;

namespace TheGramFeed
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFirebaseAuthentication(Configuration["Firebase:Issuer"], Configuration["Firebase:Audience"]);
            services.AddMemoryCache();
            
            services.AddDbContext<FeedContext>(builder =>
            {
                builder.UseInMemoryDatabase(Configuration.GetConnectionString("FeedContext"));
            });
            //ConfigureConsul(services);
            services.AddScoped<IUserContextHelper, UserContextHelper>();
            services.AddSingleton<RabbitMqPersistentConn>();
            services.AddHttpContextAccessor();
            
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            


            app.UseRabbitListener();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
    
    public static class ApplicationBuilderExtensions
    {
        public static RabbitMqPersistentConn Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RabbitMqPersistentConn>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static void OnStarted()
        {
            Listener.CreatePersistentChannels();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}