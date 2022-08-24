namespace WorkHoursBot.Model.Models;

public class Job
{
    public int Id { get; set; }
    
    public long ChatId { get; set; }
    
    public string Date { get; set; }

    public string Tasks { get; set; }
}