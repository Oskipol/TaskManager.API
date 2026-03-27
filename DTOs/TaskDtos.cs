public record CreateTaskDto(string Title, string Description, string Status);
public record UpdateTaskDto(int Id, string Title, string Description, string Status, int Order, int BoardId);