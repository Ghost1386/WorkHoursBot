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
            _timeStart = TimeOnly.FromDateTime(DateTime.Now);
            
            await botClient.SendTextMessageAsync(message.Chat.Id, "Stopwatch started.", replyMarkup: KeyboardStopwatch());
            return;
        }

        if (userMessage == "stop")
        {
            _timeStop = TimeOnly.FromDateTime(DateTime.Now);

            CreateScheduleViewModel model = new CreateScheduleViewModel
            {
                ChatId = message.Chat.Id,
                Date = Convert.ToString(DateOnly.FromDateTime(DateTime.Now.Date)),
                From = _timeStart,
                To = _timeStop,
            };
            
            _schedulesService.Create(model);

            _timeStart = TimeOnly.MinValue;
            
            await botClient.SendTextMessageAsync(message.Chat.Id, "Stopwatch stopped.", replyMarkup: KeyboardMain());
            return;
        }

        if (userMessage.Length == 8 && userMessage == "add work")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Write Add work and work example:\nAdd work\nCoding", replyMarkup: KeyboardMain());
            return;
        }
        
        if (userMessage.Length > 8 && userMessage[..8] == "add work")
        {
            var array = userMessage.Split('\n');
            var request = array.ToList();

            var model = new CreateTasksViewModel
            {
                ChatId = message.Chat.Id,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Tasks = request[^1]
            };

            await botClient.SendTextMessageAsync(message.Chat.Id, _jobsService.Create(model), replyMarkup: KeyboardMain());
            return;
        }
        
        if (userMessage.Length == 8 && userMessage == "add time")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Write Add time and time example:\nAdd time\n1:00", replyMarkup: KeyboardMain());
            return;
        }
        
        if (userMessage.Length > 8 && userMessage[..8] == "add time")
        {
            var arrayMessage = userMessage.Split('\n');
            var request = arrayMessage.ToList();

            var time = request[^1].Split(':');

            var model = new TimingScheduleViewModel()
            {
                ChatId = message.Chat.Id,
                Date = Convert.ToString(DateOnly.FromDateTime(DateTime.Now.Date)),
                Timing = Convert.ToString(new TimeSpan(Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0)),
            };

            await botClient.SendTextMessageAsync(message.Chat.Id, _schedulesService.AddTiming(model), replyMarkup: KeyboardMain());
            return;
        }
        
        if (userMessage.Length == 11 && userMessage == "remove time")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Write Remove time and time example:\nRemove time\n1:00", replyMarkup: KeyboardMain());
            return;
        }
        
        if (userMessage.Length > 11 && userMessage[..11] == "remove time")
        {
            var arrayMessage = userMessage.Split('\n');
            var request = arrayMessage.ToList();

            var time = request[^1].Split(':');

            var model = new TimingScheduleViewModel()
            {
                ChatId = message.Chat.Id,
                Date = Convert.ToString(DateOnly.FromDateTime(DateTime.Now.Date)),
                Timing = Convert.ToString(new TimeSpan(Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0)),
            };

            await botClient.SendTextMessageAsync(message.Chat.Id, _schedulesService.RemoveTiming(model), replyMarkup: KeyboardMain());
            return;
        }

        if (userMessage == "daily report")
        {
            List<string> list = _reportService.Daily();
            var response = string.Empty;

            foreach (var item in list)
            {
                response += item;
                response += "\n";
            }
            
            await botClient.SendTextMessageAsync(message.Chat.Id, $"Daily report:\n{response}", replyMarkup: KeyboardMain());
            return;
        }

        if (userMessage == "monthly report")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Choose month", replyMarkup: KeyboardMonth());
            return;
        }

        if (userMessage == "current")
        {
            List<string> list = _reportService.MonthCurrent();
            var response = string.Empty;

            foreach (var item in list)
            {
                response += item;
                response += "\n";
            }
            
            await botClient.SendTextMessageAsync(message.Chat.Id, $"Month report:\n{response}", replyMarkup: KeyboardMonth());
            return;
        }
        
        if (userMessage == "previous")
        {
            List<string> list = _reportService.MonthPrevious();
            var response = string.Empty;

            foreach (var item in list)
            {
                response += item;
                response += "\n";
            }
            
            await botClient.SendTextMessageAsync(message.Chat.Id, $"Month report:\n{response}", replyMarkup: KeyboardMonth());
            return;
        }

        if (userMessage == "back")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Choose command:", replyMarkup: KeyboardMain());
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "I don't have such a team.", replyMarkup: KeyboardMain());
            return;
        }
    }
    
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Ошибка телеграм АПИ:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
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
            new KeyboardButton[] {"Current", "Previous"},
            new KeyboardButton[] {"Back"}
        })
        {
            ResizeKeyboard = true
        };  
            
        return keyboardMain;
    }
}