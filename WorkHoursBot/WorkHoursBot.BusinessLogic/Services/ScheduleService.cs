using AutoMapper;
using WorkHoursBot.BusinessLogic.Interfaces;
using WorkHoursBot.Common.ViewModels;
using WorkHoursBot.Model;
using WorkHoursBot.Model.Models;

namespace WorkHoursBot.BusinessLogic.Services;

public class ScheduleService : IScheduleService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public ScheduleService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public string Create(CreateScheduleViewModel model)
    {
        try
        {
            var schedule = _mapper.Map<CreateScheduleViewModel, Schedule>(model);

            _context.Schedules.Add(schedule);
            _context.SaveChanges();
            return "Schedule added successfully";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Something went wrong. Try again.";
        }
    }

    public string ChangeTiming(TimingScheduleViewModel model)
    {
        try
        {
            var schedule = _mapper.Map<TimingScheduleViewModel, Schedule>(model);
        
            IQueryable<Schedule> query = _context.Schedules.AsQueryable();
            
            query = query.Where(x => x.ChatId == model.ChatId);
            query = query.Where(x => x.Date == DateOnly.FromDateTime(DateTime.UtcNow));

            if (query != null)
            {
                var record = query.SingleOrDefault();

                TimeOnly change = new TimeOnly();
                change.Add(schedule.Timing);

                record.Timing.Add(schedule.Timing);
            
                _context.Schedules.Update(record);
                _context.SaveChanges();
                return "Added successfully";
            }
            else
            {
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
                return "Added successfully";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Something went wrong. Try again.";
        }
    }
}