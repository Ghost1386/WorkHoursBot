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

    public string AddTiming(TimingScheduleViewModel model)
    {
        return ChangeTiming(model, true);
    }

    public string RemoveTiming(TimingScheduleViewModel model)
    {
        return ChangeTiming(model, false);
    }

    private string ChangeTiming(TimingScheduleViewModel model, bool type)
    {
        try
        {
            var timetable = _mapper.Map<TimingScheduleViewModel, Timetable>(model);

            IQueryable<Timetable> query = _context.Timetables.AsQueryable();

            string date = Convert.ToString(DateOnly.FromDateTime(DateTime.Now));

            query = query.Where(x => x.ChatId == timetable.ChatId);
            query = query.Where(x => x.Date == date);

            List<Timetable> list = query.ToList();

            if (list.Count == 1)
            {
                var record = query.SingleOrDefault();

                TimeSpan change = TimeSpan.Parse(timetable.Timing);

                TimeOnly recordTime = TimeOnly.Parse(record.Timing);

                TimeOnly newTime;

                newTime = type == true ? recordTime.Add(change) : recordTime.Add(-change);

                record.Timing = Convert.ToString(newTime);

                _context.Timetables.Update(record);
                _context.SaveChanges();
                
                if (type == true)
                {
                    return "Added successfully";
                }
                
                return "Removed successfully";
            }
            else
            {
                if (type == false)
                {
                    return "You can't remove time, you have to add it first";
                }
                
                _context.Timetables.Add(timetable);
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