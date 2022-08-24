namespace WorkHoursBot.Model.Models;

public class Schedule
{
    public int Id { get; set; }
    
    public long ChatId { get; set; }
    
    public string Date { get; set; }
    
    public string From { get; set; }
    
    public string To { get; set; }
    
    public string Timing { get; set; }
}