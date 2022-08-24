using WorkHoursBot.Model.Models;

namespace WorkHoursBot.BusinessLogic.Interfaces;

public interface IReportService
{
    List<string> Daily();
    
    List<string> MonthCurrent();

    List<string> MonthPrevious();
}