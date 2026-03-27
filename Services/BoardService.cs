using System.CodeDom.Compiler;
using TaskManager.API.Models;

public class BoardService
{
    private readonly AppDbContext _db;
    public BoardService(AppDbContext db)=> _db=db;
    public async Task<Board> Create(string name, int ownerId)
    {
        var board=new Board{Name=name, OwnerId=ownerId, Code=GenerateCode()};
        _db.Boards.Add(board);
        _db.BoardMembers.Add(new BoardMember{Board=board, UserId=ownerId});
        await _db.SaveChangesAsync();
        return board;
    }
    public async Task<Board?> Join(string code, int userId)
    {
        var board=_db.Boards.FirstOrDefault(b=>b.Code==code);
        if(board==null) return null;
        var exists=_db.BoardMembers.Any(bm=>bm.BoardId==board.Id&&bm.UserId==userId);
        if(exists) return board;
        _db.BoardMembers.Add(new BoardMember{UserId=userId, BoardId=board.Id});
        await _db.SaveChangesAsync();
        return board;
    }
    public List<Board> GetUserBoards(int userId)
    {
        return _db.BoardMembers.Where(bm=>bm.UserId==userId).Select(bm=>bm.Board).ToList();
    }
    private static string GenerateCode()=>Guid.NewGuid().ToString()[..8].ToUpper();
}