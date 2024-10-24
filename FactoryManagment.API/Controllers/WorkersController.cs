using FactoryManagment.API.Contracts.Workers;
using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Interfaces.Services;
using FactoryManagment.Domain.Models;
using FactoryManagment.Domain.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoryManagment.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkersController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWorkersService _workersService;

    public WorkersController(IHttpContextAccessor httpContextAccessor, IWorkersService workersService)
    {
        _httpContextAccessor = httpContextAccessor;
        _workersService = workersService;
    }

    private List<WorkerResponse> CreateWorkerResponse(List<Worker> workers)
    {
        return workers.Select(w => new WorkerResponse(
        
            w.Id,
            w.FirstName,
            w.LastName,
            w.Email,
            w.PhoneNumber,
            w.DateOfBirth.UtcDateTime,
            w.Job.ToString()
        )).ToList();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<WorkerResponse>>> GetAllWorkers()
    {
        var workers = await _workersService.GetAllWorkersAsync(_httpContextAccessor.HttpContext!);

        var response = CreateWorkerResponse(workers);
        
        return Ok(response);
    }

    [Authorize]
    [HttpGet("{identifier}")] 
    public async Task<ActionResult<List<WorkerResponse>>> GetWorker(string identifier)
    {
        if (Enum.TryParse<Jobs>(identifier, true, out var job))
        {
            var workers = await _workersService.GetAllByJobWorkersAsync(_httpContextAccessor.HttpContext!, job);
            
            var jobResponse = CreateWorkerResponse(workers);
            
            return Ok(jobResponse);
        }
        
        var worker = await _workersService.GetWorkerByIdentifierAsync(_httpContextAccessor.HttpContext!, identifier);
        
        return Ok(worker);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddWorker([FromBody] WorkerRequest request)
    {
        if(!DateTimeOffset.TryParse(request.DateOfBirth + " 00:00:00.000000  +00:00", out var dateOfBirth))
            return BadRequest("Invalid date of birth");
        
        if(!Enum.TryParse<Jobs>(request.Job, true, out var job))
            return BadRequest("Invalid job");

        var (error, worker) = Worker.Create(
            Guid.NewGuid(),
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            dateOfBirth,
            job);
        
        if(error != string.Empty)
            return BadRequest(error);
        
        error = await _workersService.CreateWorkerAsync(_httpContextAccessor.HttpContext!, worker);
        
        if(error != string.Empty)
            return BadRequest(error);
        
        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateWorker([FromBody] UpdateWorkerRequest request)
    {
        if(!Enum.TryParse<Jobs>(request.Job, true, out var job))
            return BadRequest("Invalid job");
        
        var error = UpdateWorkerRequestValidator.Validate(request.Email, request.PhoneNumber);
        if(error != string.Empty)
            return BadRequest(error);
        
        await _workersService.UpdateWorkerAsync(_httpContextAccessor.HttpContext!, request.Id, request.Email, request.PhoneNumber, job);
        
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id::guid}")]
    public async Task<ActionResult> DeleteWorker(Guid id)
    {
        await _workersService.DeleteWorkerAsync(_httpContextAccessor.HttpContext!, id);
        
        return Ok();
    }
}