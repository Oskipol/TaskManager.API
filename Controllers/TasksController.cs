using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _task;
    private readonly IHubContext<TaskHub> _hub;
    public TasksController(TaskService task, IHubContext<TaskHub> hub){
        _task=task;
        _hub=hub;
    
    }
    [HttpGet("{boardId}")]
    public IActionResult GetTasks(int boardId)=>Ok(_task.GetBoardTasks(boardId));
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        var task=await _task.Create(dto);
        await _hub.Clients.Group($"board-{dto.BoardId}").SendAsync("TaskCreated", task);
        return Ok(task);
    }
    [HttpPut]
    public async Task<IActionResult> Update(UpdateTaskDto dto)
    {
        var task=await _task.Update(dto);
        if(task==null) return NotFound("Task not found");
        await _hub.Clients.Group($"board-{dto.BoardId}").SendAsync("TaskUpdated", task);
        return Ok(task);
    }
}