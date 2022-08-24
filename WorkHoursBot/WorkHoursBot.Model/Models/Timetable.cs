namespace WorkHoursBot.Model.Models;

public class Timetable
{
    public int Id { get; set; }
    
    public long ChatId { get; set; }
    
    public string Date { get; set; }

    public string Timing { get; set; }
}