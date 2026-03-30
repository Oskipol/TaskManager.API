using System.CodeDom.Compiler;
using TaskManager.API.Models;
using Microsoft.EntityFrameworkCore;

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
    public List <string> GetBoardMembers(int boardId)
    {
        return _db.BoardMembers.Where(bm=>bm.BoardId==boardId).Select(bm=>bm.User.Username).ToList();
    }
    public string? GetBoardOwner(int boardId)
    {
        return _db.Boards
            .Where(b => b.Id == boardId)
            .Select(b => b.Owner != null ? b.Owner.Username : null)
            .FirstOrDefault();
    }
    public string? GetBoardLeader(int boardId)
    {
        return _db.Boards
            .Where(b => b.Id == boardId)
            .Select(b => b.Leader != null ? b.Leader.Username : null)
            .FirstOrDefault();
    }
    public async Task<Board?> SetBoardLeader(int boardId, string leaderUsername)
    {
        var board = await _db.Boards.FindAsync(boardId);
        if (board == null) return null;
        var memberExists = _db.BoardMembers.Any(bm => bm.BoardId == boardId && bm.User.Username == leaderUsername);
        if (!memberExists) return null;
        var user = await _db.Users.FirstOrDefaultAsync<User>(u => u.Username == leaderUsername);
        if (user == null) return null;
        board.Leader = user;
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