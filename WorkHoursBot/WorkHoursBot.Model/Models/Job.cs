namespace WorkHoursBot.Model.Models;

public class Job
{
    public long ChatId { get; set; }
    
    public DateOnly Date { get; set; }

    public string Tasks { get; set; }
}