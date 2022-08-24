using AutoMapper;
using WorkHoursBot.Common.ViewModels;
using WorkHoursBot.Model.Models;

namespace WorkHoursBot.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateTasksViewModel, Job>();

        CreateMap<CreateScheduleViewModel, Schedule>();

        CreateMap<TimingScheduleViewModel, Timetable>();
    }
}