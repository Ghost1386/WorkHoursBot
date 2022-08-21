using WorkHoursBot.BusinessLogic.Interfaces;
using WorkHoursBot.Model;
using WorkHoursBot.Model.Models;

namespace WorkHoursBot.BusinessLogic.Services;

public class ReportService : IReportService
{
    private readonly ApplicationContext _context;

    public ReportService(ApplicationContext context)
    {
        _context = context;
    }
    
    public List<Schedule> Daily()
    {
        try
        {
            IQueryable<Schedule> query = _context.Schedules.AsQueryable();
        
            query = query.Where(x => x.Date == DateOnly.FromDateTime(DateTime.Now));

            return query.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<Schedule>();
        }
    }

    public List<string> MonthlyNow()
    {
        try
        {
            IQueryable<Schedule> querySchedule = _context.Schedules.AsQueryable();
            IQueryable<Job> queryJobs = _context.Jobs.AsQueryable();

            List<DateOnly> dates = LimitedDay();
            List<string> info = new List<string?>();

            foreach (var date in dates)
            {
                info.Add(querySchedule.Where(x => x.Date == date).ToString());
                info.Add(queryJobs.Where(x =>x.Date == date).ToString());
            }
        
            return info;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<string>();
        }
    }

    public List<string> MonthPrevious()
    {
        try
        {
            IQueryable<Schedule> querySchedule = _context.Schedules.AsQueryable();
            IQueryable<Job> queryJobs = _context.Jobs.AsQueryable();

            List<DateOnly> dates = UnLimitedDay();
            List<string> info = new List<string>();

            foreach (var date in dates)
            {
                info.Add(querySchedule.Where(x => x.Date == date).ToString());
                info.Add(queryJobs.Where(x =>x.Date == date).ToString());
            }
        
            return info;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<string>();
        }
    }

    private List<DateOnly> LimitedDay()
    {
        List<DateOnly> dates = new List<DateOnly>();

        for (int i = 1; i <= DateTime.Now.Day; i++)
        {
            dates.Add(DateOnly.Parse($"{i}.{DateTime.UtcNow.Month}." +
                                     $"{DateTime.UtcNow.Year}"));
        }

        return dates;
    }

    private List<DateOnly> UnLimitedDay()
    {
        List<DateOnly> dates = new List<DateOnly>();

        for (int i = 1; i <= DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month-1); i++)
        {
            dates.Add(DateOnly.Parse($"{i}.{DateTime.UtcNow.Month}." +
                                     $"{DateTime.UtcNow.Year}"));
        }

        return dates;
    }
}