using WorkHoursBot.Common.ViewModels;

namespace WorkHoursBot.BusinessLogic.Interfaces;

public interface IJobsService
{
    string Create(CreateTasksViewModel model);
}