using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using WorkHoursBot.BusinessLogic.Interfaces;
using WorkHoursBot.BusinessLogic.Services;
using WorkHoursBot.Mapper;
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
                    services.AddTransient<IJobsService, JobsService>();
                    services.AddTransient<IReportService, ReportService>();
                    services.AddTransient<IScheduleService, ScheduleService>();
                    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
                    services.AddSingleton(mapper);
                })
                .Build();
            
            ActivatorUtilities.CreateInstance<IJobsService>(host.Services);
            ActivatorUtilities.CreateInstance<IReportService>(host.Services);
            ActivatorUtilities.CreateInstance<IScheduleService>(host.Services);

            var jobsService = ActivatorUtilities.CreateInstance<IJobsService>(host.Services);
            var reportService = ActivatorUtilities.CreateInstance<IReportService>(host.Services);
            var scheduleService = ActivatorUtilities.CreateInstance<IScheduleService>(host.Services);
            
            var messageController = new MessageController(jobsService, reportService, scheduleService);
            
            var botClient = new TelegramBotClient("5663935981:AAE0Qm05seCXuvGoZUlF_wi8cPkTb1zgGt4");

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

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