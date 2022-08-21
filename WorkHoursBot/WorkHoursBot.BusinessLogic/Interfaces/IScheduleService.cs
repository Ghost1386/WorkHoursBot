using WorkHoursBot.Common.ViewModels;

namespace WorkHoursBot.BusinessLogic.Interfaces;

public interface IScheduleService
{
    string Create(CreateScheduleViewModel model);
    
    string ChangeTiming(TimingScheduleViewModel model);
}