using WorkHoursBot.BusinessLogic.Interfaces;
using WorkHoursBot.Common.ViewModels;
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
    
    public List<string> Daily(ReportViewModel model)
    {
        try
        {
            IQueryable<Schedule> querySchedule = _context.Schedules.AsQueryable();
            IQueryable<Timetable> queryTimetables = _context.Timetables.AsQueryable();
            IQueryable<Job> queryJobs = _context.Jobs.AsQueryable();
            
            string date = Convert.ToString(DateOnly.FromDateTime(DateTime.Now));

            querySchedule = querySchedule.Where(x => x.ChatId == model.ChatId);
            querySchedule = querySchedule.Where(x => x.Date == date);
            string schedule = string.Empty;
            if (querySchedule.FirstOrDefault() != null)
            {
                List<Schedule> schedules = querySchedule.ToList();
                foreach (var s in schedules)
                {
                    schedule += s.From + "-" + s.To + "\n";
                }
            }
            
            queryTimetables = queryTimetables.Where(x => x.ChatId == model.ChatId);
            queryTimetables = queryTimetables.Where(x => x.Date == date);
            string timetable = string.Empty;
            if (queryTimetables.FirstOrDefault() != null)
            {
                List<Timetable> timetables = queryTimetables.ToList();
                foreach (var t in timetables)
                {
                    timetable += t.Timing + "\n";
                }
            }
            
            
            queryJobs = queryJobs.Where(x => x.ChatId == model.ChatId);
            queryJobs = queryJobs.Where(x => x.Date == date);
            string job = String.Empty;
            if (queryJobs.FirstOrDefault() != null)
            {
                List<Job> jobs = queryJobs.ToList();
                foreach (var j in jobs)
                {
                    job += j.Tasks + "\n";
                }
            }

            List<string> info = new List<string>
            {
                date,
                "---",
                schedule,
                "---",
                timetable,
                "---",
                job,
            };

            return info;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<string>();
        }
    }

    public List<string> MonthCurrent(ReportViewModel model)
    {
        try
        {
            List<DateOnly> dates = LimitedDay();

            return GetInfo(model.ChatId, dates);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<string>();
        }
    }
    
    public List<string> MonthPrevious(ReportViewModel model)
    {
        try
        {
            List<DateOnly> dates = UnLimitedDay();

            return GetInfo(model.ChatId, dates);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<string>();
        }
    }

    private List<string> GetInfo(long id, List<DateOnly> dates)
    {
        List<string> info = new List<string>();
        
        foreach (var date in dates)
        {
            IQueryable<Schedule> querySchedule = _context.Schedules.AsQueryable();
            IQueryable<Timetable> queryTimetables = _context.Timetables.AsQueryable();
            IQueryable<Job> queryJobs = _context.Jobs.AsQueryable();

            querySchedule = querySchedule.Where(x => x.ChatId == id);
            querySchedule = querySchedule.Where(x => x.Date == Convert.ToString(date));
            string schedule = string.Empty;
            if (querySchedule.FirstOrDefault() != null)
            {
                List<Schedule> schedules = querySchedule.ToList();
                foreach (var s in schedules)
                {
                    schedule += s.From + "-" + s.To + "\n";
                }
            }

            queryTimetables = queryTimetables.Where(x => x.ChatId == id);
            queryTimetables = queryTimetables.Where(x => x.Date == Convert.ToString(date));
            string timetable = string.Empty;
            if (queryTimetables.FirstOrDefault() != null)
            {
                List<Timetable> timetables = queryTimetables.ToList();
                foreach (var t in timetables)
                {
                    timetable += t.Timing + "\n";
                }
            }

            queryJobs = queryJobs.Where(x => x.ChatId == id);
            queryJobs = queryJobs.Where(x => x.Date == Convert.ToString(date));
            string job = String.Empty;
            if (queryJobs.FirstOrDefault() != null)
            {
                List<Job> jobs = queryJobs.ToList();
                foreach (var j in jobs)
                {
                    job += j.Tasks + "\n";
                }
            }

            info.Add(date.ToString());
            info.Add("---");
            info.Add(schedule);
            info.Add("---");
            info.Add(timetable);
            info.Add("---");
            info.Add(job);
            info.Add("===");
        }

        return info;
    }

    private List<DateOnly> LimitedDay()
    {
        List<DateOnly> dates = new List<DateOnly>();

        for (int i = 1; i <= DateTime.Now.Day; i++)
        {
            dates.Add(DateOnly.Parse($"{DateTime.Now.Month}/{i}/" +
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