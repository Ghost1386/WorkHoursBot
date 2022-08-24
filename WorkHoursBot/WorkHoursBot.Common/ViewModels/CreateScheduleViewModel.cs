using System.ComponentModel.DataAnnotations;

namespace WorkHoursBot.Common.ViewModels;

public class CreateScheduleViewModel
{
    [Required]
    public long ChatId { get; set; }
    
    [Required]
    public string Date { get; set; }
    
    [Required]
    public TimeOnly From { get; set; }
    
    [Required]
    public TimeOnly To { get; set; }
}