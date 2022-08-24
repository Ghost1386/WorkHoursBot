using System.ComponentModel.DataAnnotations;

namespace WorkHoursBot.Common.ViewModels;

public class TimingScheduleViewModel
{
    [Required]
    public long ChatId { get; set; }
    
    [Required]
    public string Date { get; set; }
    
    [Required]
    public string Timing { get; set; }
}