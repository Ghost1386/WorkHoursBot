using System.ComponentModel.DataAnnotations;

namespace WorkHoursBot.Common.ViewModels;

public class CreateTasksViewModel
{
    [Required]
    public long ChatId { get; set; }
    
    [Required]
    public DateOnly Date { get; set; }
    
    [Required]
    public string Tasks { get; set; }
}