using DemoDotnetBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Telegram.Bot;
using TelegramBotEnglishDb.DbContexts;

namespace TelegramBotEnglishDb.Configurations
{
    public static class ServicesConfiguration
    {
        public static void AddConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add PostgreSQL database context
            string connectionString = configuration.GetConnectionString("DatabaseConnection")!;
            services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connectionString));

            // Add Telegram Bot client
            var token = configuration.GetSection("BotConfiguration").GetValue("BotToken", string.Empty);
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token));

            services.AddControllers();
        }
    }
}
