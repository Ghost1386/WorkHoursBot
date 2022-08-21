namespace WorkHoursBot.Model.Models;

public class Schedule
{
    public long ChatId { get; set; }
    
    public DateOnly Date { get; set; }
    
    public TimeOnly From { get; set; }
    
    public TimeOnly To { get; set; }
    
    public TimeSpan Timing { get; set; }
}