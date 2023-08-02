using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBotEnglishDb.Configurations;
using TelegramBotEnglishDb.DbContexts;
using TelegramBotEnglishDb.Interfaces.UserInterfaces;
using TelegramBotEnglishDb.Services;
//using TelegramBotEnglishDb.Services;

namespace TelegramBotEnglishDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add token 
            var token = builder.Configuration.GetSection("BotConfiguration").GetValue("BotToken", string.Empty);
            //var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();//added
            //var botToken = botConfig.BotToken ?? string.Empty;//added
            builder.Services.AddSingleton(p => new TelegramBotClient(token));

            builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
            builder.Services.AddHostedService<BotBackgroundService>();
            builder.Services.AddScoped<IUserService, UserService>();
            //database
            builder.Services.AddDbContext<UserDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddHttpClient("TelegramWebhook")//added
            .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(token, httpClient));//added
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
/*
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Examples.Minimal.API.Services;
using Telegram.Bot.Polling;
using TelegramBotEnglishDb.Configurations;
using TelegramBotEnglishDb.DbContexts;
using TelegramBotEnglishDb.Interfaces.UserInterfaces;
using TelegramBotEnglishDb.Services;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BotConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        }

        public IConfiguration Configuration { get; }
        public BotConfiguration BotConfig { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var token = Configuration.GetSection("BotConfiguration").GetValue("BotToken", string.Empty);
            services.AddSingleton(p => new TelegramBotClient(token));
            services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
            services.AddHostedService<BotBackgroundService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ConfigureWebhook>();
            //database
            services.AddDbContext<UserDbContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddHostedService<ConfigureWebhook>();

            services.AddHttpClient("tgwebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient =>
                    new TelegramBotClient(BotConfig.BotToken!, httpClient));

            services.AddScoped<BotUpdateHandler>();

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

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                var token = BotConfig.BotToken;

                endpoints.MapControllerRoute(name: "tgwebhook",
                    pattern: $"bot/{token}",
                    new { controller = "Webhook", action = "Post" });

                endpoints.MapControllers();
            });
        }
    }
}*/

/*
using Telegram.Bot.Examples.Minimal.API.Services;
using Telegram.Bot;
using TelegramBotEnglishDb.Configurations;
using Telegram.Bot.Examples.Minimal.API;
using TelegramBotEnglishDb.Services;
using System.Threading;
using Telegram.Bot.Polling;
using TelegramBotEnglishDb.Interfaces.UserInterfaces;
using Microsoft.Extensions.Configuration;
using TelegramBotEnglishDb.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
var botToken = botConfig.BotToken ?? string.Empty;

// There are several strategies for completing asynchronous tasks during startup.
// Some of them could be found in this article https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
// We are going to use IHostedService to add and later remove Webhook
builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
//builder.Services.AddHostedService<BotBackgroundService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHostedService<ConfigureWebhook>();
builder.Services.AddDbContext<UserDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register named HttpClient to get benefits of IHttpClientFactory
// and consume it with ITelegramBotClient typed client.
// More read:
//  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0#typed-clients
//  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
builder.Services.AddHttpClient("TelegramWebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botToken, httpClient));

// Dummy business-logic service
builder.Services.AddScoped<BotUpdateHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpLogging();

// Configure custom endpoint per Telegram API recommendations:
// https://core.telegram.org/bots/api#setwebhook
// If you'd like to make sure that the Webhook request comes from Telegram, we recommend
// using a secret path in the URL, e.g. https://www.example.com/<token>.
// Since nobody else knows your bot's token, you can be pretty sure it's us.
app.MapPost($"/bot/{botConfig.EscapedBotToken}", async (
    ITelegramBotClient botClient,
    HttpRequest request,
    CancellationToken cancellationToken,
    BotUpdateHandler handleUpdateService,
    NewtonsoftJsonUpdate update) =>
{
    if (update.Message == null)
    {
        throw new ArgumentException(nameof(update.Message));
    }

    await handleUpdateService.HandleUpdateAsync(botClient,update, cancellationToken);

    return Results.Ok();
})
.WithName("TelegramWebhook");

app.Run();*/
