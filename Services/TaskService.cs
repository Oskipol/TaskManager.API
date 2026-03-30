using TaskManager.API.Models;

public class TaskService
{
    private readonly AppDbContext _db;
    public TaskService(AppDbContext db)=> _db=db;
    public List<TaskItem> GetBoardTasks(int boardId)=>_db.Tasks.Where(t=>t.BoardId==boardId).ToList();

    public async Task<TaskItem> Create(CreateTaskDto dto)
    {
        var task=new TaskItem{Title=dto.Title, BoardId=dto.BoardId, AssignedTo=dto.AssignedTo, Description=dto.Description, Status="todo", Order=_db.Tasks.Count(t=>t.BoardId==dto.BoardId)};
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }
    public async Task<TaskItem?> Update(UpdateTaskDto dto)
    {
        var task=await _db.Tasks.FindAsync(dto.Id);
        if(task==null) return null;
        task.Title=dto.Title;
        task.AssignedTo=dto.AssignedTo;
        task.Description=dto.Description;
        task.Status=dto.Status;
        task.Order=dto.Order;
        await _db.SaveChangesAsync();
        return task;
    }
}