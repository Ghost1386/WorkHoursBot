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
    
    public List<string> Daily()
    {
        try
        {
            IQueryable<Schedule> querySchedule = _context.Schedules.AsQueryable();
            IQueryable<Timetable> queryTimetables = _context.Timetables.AsQueryable();
            IQueryable<Job> queryJobs = _context.Jobs.AsQueryable();

            List<string> info = new List<string>
            {
                DateTime.UtcNow.ToString(),
                "---",
                querySchedule.Where(x => x.Date == Convert.ToString(DateOnly.FromDateTime(DateTime.Now))).ToString(),
                "---",
                queryTimetables.Where(x => x.Date == Convert.ToString(DateOnly.FromDateTime(DateTime.Now))).ToString(),
                "---",
                queryJobs.Where(x => x.Date == Convert.ToString(DateOnly.FromDateTime(DateTime.Now))).ToString(),
                "==="
            };

            return info;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<string>();
        }
    }

    public List<string> MonthCurrent()
    {
        try
        {
            IQueryable<Schedule> querySchedule = _context.Schedules.AsQueryable();
            IQueryable<Timetable> queryTimetables = _context.Timetables.AsQueryable();
            IQueryable<Job> queryJobs = _context.Jobs.AsQueryable();

            List<DateOnly> dates = LimitedDay();
            List<string> info = new List<string>();

            foreach (var date in dates)
            {
                info.Add(date.ToString());
                info.Add("---");
                info.Add(querySchedule.Where(x => x.Date == Convert.ToString(date)).ToString());
                info.Add("---");
                info.Add(queryTimetables.Where(x => x.Date == Convert.ToString(date)).ToString());
                info.Add("---");
                info.Add(queryJobs.Where(x =>x.Date == Convert.ToString(date)).ToString());
                info.Add("===");
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
            IQueryable<Timetable> queryTimetables = _context.Timetables.AsQueryable();
            IQueryable<Job> queryJobs = _context.Jobs.AsQueryable();

            List<DateOnly> dates = UnLimitedDay();
            List<string> info = new List<string>();

            foreach (var date in dates)
            {
                info.Add(date.ToString());
                info.Add("---");
                info.Add(querySchedule.Where(x => x.Date == Convert.ToString(date)).ToString());
                info.Add("---");
                info.Add(queryTimetables.Where(x => x.Date == Convert.ToString(date)).ToString());
                info.Add("---");
                info.Add(queryJobs.Where(x => x.Date == Convert.ToString(date)).ToString());
                info.Add("===");
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
            dates.Add(DateOnly.Parse($"{i}.{DateTime.Now.Month}." +
                                     $"{DateTime.Now.Year}"));
        }

        return dates;
    }

    private List<DateOnly> UnLimitedDay()
    {
        List<DateOnly> dates = new List<DateOnly>();

        for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month-1); i++)
        {
            dates.Add(DateOnly.Parse($"{i}.{DateTime.Now.Month}." +
                                     $"{DateTime.Now.Year}"));
        }

        return dates;
    }
}