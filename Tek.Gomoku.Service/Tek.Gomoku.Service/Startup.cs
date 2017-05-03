using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Tek.Gomoku.Service.Models;
using Tek.Gomoku.Service.Services;

namespace Tek.Gomoku.Service
{
    public class Startup
    {
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
            services.AddTransient<IdentityInitializer>();
            services.AddTransient<GameInitializer>();
            services.AddSingleton(Configuration);
            services.AddTransient(typeof(IUserInfoService), typeof(UserInfoService));
            services.AddTransient(typeof(IGameJudgementService), typeof(GameJudgementService));
            services.AddTransient(typeof(IGameService), typeof(GameService));
            services.AddTransient(typeof(IJWTService), typeof(JWTService));
            services.AddTransient(typeof(IAutoPlayService), typeof(AutoPlayService));
            services.AddTransient(typeof(IGameMoveDataAdapter), typeof(GameMoveDataAdapter));

            services.AddDbContext<GameContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GameContext")));

            services.AddIdentity<Player, IdentityRole>().AddEntityFrameworkStores<GameContext>();

            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Add framework services.
            services.AddMvc();

            services.Add(new ServiceDescriptor(typeof(ISocketService), _socketService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IdentityInitializer identityInitializer,
            GameInitializer gameInitializer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        await _socketService.AddSocket(context.WebSockets);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

            // global policy - assign here or on each controller
            app.UseCors("CorsPolicy");

            app.UseIdentity();

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateLifetime = true
                }
            });

            app.UseMvc();

            app.UseFileServer();

            identityInitializer.Seed().Wait();
            gameInitializer.Seed().Wait();
        }

        private ISocketService _socketService = new SocketService();
    }
}
