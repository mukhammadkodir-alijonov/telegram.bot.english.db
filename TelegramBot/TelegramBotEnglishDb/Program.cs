using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
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