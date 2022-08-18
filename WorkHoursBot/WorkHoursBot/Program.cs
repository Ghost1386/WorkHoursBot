using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using WorkHoursBot.Common;
using WorkHoursBot.Model;

namespace WorkHoursBot
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            string connection = "Server=localhost;Database=WorkHoursBot;Trusted_Connection=True;";
            
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //services.AddTransient<IUserService, UserService>();
                    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
                    services.AddSingleton(mapper);
                })
                .Build();
            
            //ActivatorUtilities.CreateInstance<UserService>(host.Services);

            //_userService = ActivatorUtilities.CreateInstance<UserService>(host.Services);
            
            var botClient = new TelegramBotClient("5663935981:AAE0Qm05seCXuvGoZUlF_wi8cPkTb1zgGt4");

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            var messageController = new MessageController();
            
            botClient.StartReceiving(
                messageController.HandleUpdatesAsync,
                messageController.HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Бот запущен @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }
    }
}