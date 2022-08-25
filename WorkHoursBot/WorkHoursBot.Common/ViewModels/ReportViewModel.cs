using System.ComponentModel.DataAnnotations;

namespace WorkHoursBot.Common.ViewModels;

public class ReportViewModel
{
    [Required]
    public long ChatId { get; set; }
}