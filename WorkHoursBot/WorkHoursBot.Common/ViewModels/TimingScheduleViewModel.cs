using System.ComponentModel.DataAnnotations;

namespace WorkHoursBot.Common.ViewModels;

public class TimingScheduleViewModel
{
    [Required]
    public long ChatId { get; set; }
    
    [Required]
    public DateOnly Date { get; set; }
    
    [Required]
    public TimeOnly Timing { get; set; }
}