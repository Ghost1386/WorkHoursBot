using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WorkHoursBot.BusinessLogic.Interfaces;
using WorkHoursBot.Common.ViewModels;

namespace WorkHoursBot;

public class MessageController
{
    private static TimeOnly _timeStart;
    private static TimeOnly _timeStop;
    
    private static IJobsService _jobsService;
    private static IReportService _reportService;
    private static IScheduleService _schedulesService;

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
            await HandleMessage(botClient, update.Message);
        }
    }

    private static async Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        string userMessage = message.Text.ToLower();
        
        if (userMessage == "/start")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Choose command:", replyMarkup: KeyboardMain());
            return;
        }

        if (userMessage == "start")
        {
            _timeStart = TimeOnly.FromDateTime(DateTime.UtcNow);
            
            await botClient.SendTextMessageAsync(message.Chat.Id, "Stopwatch started.", replyMarkup: KeyboardStopwatch());
            return;
        }

        if (userMessage == "stop")
        {
            _timeStop = TimeOnly.FromDateTime(DateTime.UtcNow);

            CreateScheduleViewModel model = new CreateScheduleViewModel();
            model.ChatId = message.Chat.Id;
            model.Date = DateOnly.FromDateTime(DateTime.UtcNow);
            model.From = _timeStart;
            model.To = _timeStop;

            _schedulesService.Create(model);
            
            await botClient.SendTextMessageAsync(message.Chat.Id, "Stopwatch stopped.", replyMarkup: KeyboardMain());
            return;
        }
        
        if (userMessage == "daily report")
        {
            
        }

        if (userMessage == "monthly report")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Choose month", replyMarkup: KeyboardMonth());
            return;
        }

        if (userMessage == "current")
        {
            
        }
        
        if (userMessage == "previous")
        {
            
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
    
    private static ReplyKeyboardMarkup KeyboardMonth()
    {
        ReplyKeyboardMarkup keyboardMain = new(new[]
        {
            new KeyboardButton[] {"Current", "Previous"}
        })
        {
            ResizeKeyboard = true
        };  
            
        return keyboardMain;
    }
}