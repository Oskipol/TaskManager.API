public record CreateTaskDto(string Title, string Description, int BoardId);
public record UpdateTaskDto(int Id, string Title, string Description, string Status, int Order, int BoardId);