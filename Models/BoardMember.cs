public class BoardMember
{
    public int UserId {get; set;}
    public User User {set; get;}=null!;
    public int BoardId {get; set;}
    public Board Board {get; set;}=null!;
}