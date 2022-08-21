using Microsoft.EntityFrameworkCore;
using WorkHoursBot.Model.Models;
using Task = System.Threading.Tasks.Task;

namespace WorkHoursBot.Model;

public class ApplicationContext : DbContext
{
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}