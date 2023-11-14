using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Todo.Data.Repositories;
using Todo.Validations;
using Todo.ViewModels;

namespace Todo.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{

    private readonly ILogger<TodoController> _logger;
    private readonly TaskRepository _taskRepository;

    public TodoController(ILogger<TodoController> logger, TaskRepository taskRepository)
    {
        _logger = logger;
        _taskRepository = taskRepository;
    }

    [HttpPost]
    public IActionResult Insert([FromBody] Dtos.InsertOrUpdateTaskDto taskDto)
    {
        ValidationResult results = new InsertOrUpdateTaskDtoValidator().Validate(taskDto);

        if (!results.IsValid) return BadRequest(results.Errors);

        var task = new Data.Models.Task()
        {
            Name = taskDto.Name,
            Description = taskDto.Description,
            StartTime = taskDto.StartTime,
            FinishTime = taskDto.FinishTime
        };


        try
        {
            _taskRepository.Create(task);
            return Created($"Todo/{task.Id}", task);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpGet]
    public IActionResult GetTasks()
    {
        var tasks = _taskRepository.GetList().Select((t) => new TaskViewModel()
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            StartTime = t.StartTime,
            FinishTime = t.FinishTime,
            Active = t.Active
        }).ToList();

        if (tasks.Count > 0) return Ok(tasks);
        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var task = _taskRepository.GetById(id);

        if (task is null) return NotFound();

        return Ok(new TaskViewModel()
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            StartTime = task.StartTime,
            FinishTime = task.FinishTime,
            Active = task.Active
        });
    }

    [HttpPatch("{id}/active")]
    public IActionResult Activate([FromRoute] int id)
    {
        var taskToActivate = _taskRepository.GetById(id);
        if (taskToActivate is null) return NotFound();

        var activateTaks = _taskRepository.Query((t) => t.Active);

        activateTaks.ForEach((t) =>
        {
            t.Active = false;
            _taskRepository.Update(t);
        });

        taskToActivate.Active = true;

        try
        {
            _taskRepository.Update(taskToActivate);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPut("{id}")]
    public IActionResult Update([FromBody] Dtos.InsertOrUpdateTaskDto taskDto, int id)
    {
        var task = _taskRepository.GetById(id);

        if (task is null) return NotFound();

        task.Name = taskDto.Name;
        task.Description = taskDto.Description;
        task.StartTime = taskDto.StartTime;
        task.FinishTime = taskDto.FinishTime;

        try
        {
            _taskRepository.Update(task);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Remove([FromRoute] int id)
    {
        try
        {
            _taskRepository.Delete(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
