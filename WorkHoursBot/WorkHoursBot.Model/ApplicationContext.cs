using Microsoft.EntityFrameworkCore;

namespace WorkHoursBot.Model;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}