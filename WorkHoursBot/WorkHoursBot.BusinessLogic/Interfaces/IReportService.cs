using WorkHoursBot.Model.Models;

namespace WorkHoursBot.BusinessLogic.Interfaces;

public interface IReportService
{
    List<Schedule> Daily();
    
    List<string> MonthlyNow();

    List<string> MonthPrevious();
}