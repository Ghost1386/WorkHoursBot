using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WorkHoursBot.BusinessLogic.Interfaces;

namespace WorkHoursBot;

public class MessageController
{
    private readonly IJobsService _jobsService;
    private readonly IReportService _reportService;
    private readonly IScheduleService _schedulesService;
    public MessageController(IJobsService jobsService, IReportService reportService, IScheduleService schedulesService)
    {
        _jobsService = jobsService;
        _reportService = reportService;
        _schedulesService = schedulesService;
    }
    
    public async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            await HadleMessage(botClient, update.Message);
        }
    }

    private static async Task HadleMessage(ITelegramBotClient botClient, Message message)
    {
        string userMessage = message.Text.ToLower();
        
        if (userMessage == "/start")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Choose command:", replyMarkup: KeyboardMain());
            return;
        }
    }
    
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Ошибка телеграм АПИ:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
    
    private static ReplyKeyboardMarkup KeyboardMain()
    {
        ReplyKeyboardMarkup keyboardMain = new(new[]
        {
            new KeyboardButton[] {"Start", "Add work"},
            new KeyboardButton[] {"Add time", "Remove time"},
            new KeyboardButton[] {"Daily report", "Monthly report"}
        })
        {
            ResizeKeyboard = true
        };  
            
        return keyboardMain;
    }
    
    private static ReplyKeyboardMarkup KeyboardStopwatch()
    {
        ReplyKeyboardMarkup keyboardMain = new(new[]
        {
            new KeyboardButton[] {"Stop", "Add work"},
            new KeyboardButton[] {"Add time", "Remove time"},
            new KeyboardButton[] {"Daily report", "Monthly report"}
        })
        {
            ResizeKeyboard = true
        };  
            
        return keyboardMain;
    } 
}