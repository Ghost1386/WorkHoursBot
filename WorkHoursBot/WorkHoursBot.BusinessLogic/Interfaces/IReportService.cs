using WorkHoursBot.Common.ViewModels;
using WorkHoursBot.Model.Models;

namespace WorkHoursBot.BusinessLogic.Interfaces;

public interface IReportService
{
    List<string> Daily(ReportViewModel model);
    
    List<string> MonthCurrent(ReportViewModel model);

    List<string> MonthPrevious(ReportViewModel model);
}