public class TaskItem
{
    public int Id {get; set;}
    public string Title {get; set;}=String.Empty;
    public string Description {get; set;}=String.Empty;
    public string Status {get; set;}=String.Empty;
    public int Order {get; set;}
    public int BoardId {get; set;}
    public Board Board {get; set;}=null!;
}