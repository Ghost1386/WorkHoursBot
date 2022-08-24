using AutoMapper;
using WorkHoursBot.BusinessLogic.Interfaces;
using WorkHoursBot.Common.ViewModels;
using WorkHoursBot.Model;
using WorkHoursBot.Model.Models;

namespace WorkHoursBot.BusinessLogic.Services;

public class JobsService : IJobsService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public JobsService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public string Create(CreateTasksViewModel model)
    {
        try
        {
            var job = _mapper.Map<CreateTasksViewModel, Job>(model);

            _context.Jobs.Add(job);
            _context.SaveChanges();
            return "Job added successfully.";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Something went wrong. Try again.";
        }
    }
}