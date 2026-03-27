public class Board
{
    public int Id {get; set;}
    public string Name {get; set;}=String.Empty;
    public string Code {get; set;}=String.Empty;
    public int OwnerId {get; set;}
    public User Owner {get; set;}=null!;
    public ICollection<TaskItem> Tasks {get; set;}=[];
    public ICollection<BoardMember> Members {get; set;}=[];
}