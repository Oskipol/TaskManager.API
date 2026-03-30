using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly BoardService _boards;
    public BoardsController(BoardService boards)=>_boards=boards;
    private int UserId=>int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    [HttpGet]
    public IActionResult GetMyBoards()
    {
        return Ok(_boards.GetUserBoards(UserId).Select(b=>new BoardResponseDto(b.Id, b.Name, b.Code)));
    }
    [HttpGet("{boardId}/leader")]
    public async Task<IActionResult> GetLeader(int boardId)=>Ok(_boards.GetBoardLeader(boardId));
    [HttpPost("{boardId}/leader")]
    public async Task<IActionResult> SetLeader(int boardId, SetLeaderDto dto)
    {
        var board=await _boards.SetBoardLeader(boardId, dto.LeaderUsername);
        if(board==null) return NotFound("Board or user not found, or user is not a member");
        return Ok(new BoardResponseDto(board.Id, board.Name, board.Code));
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateBoardDto dto)
    {
        var board=await _boards.Create(dto.Name, UserId);
        return Ok(new BoardResponseDto(board.Id, board.Name, board.Code));
    }
    [HttpGet("{boardId}")]
    public async Task<IActionResult> Members(int boardId)=>Ok(_boards.GetBoardMembers(boardId));
    [HttpGet("{boardId}/owner")]
    public async Task<IActionResult> GetOwner(int boardId)=>Ok(_boards.GetBoardOwner(boardId));
    [HttpPost("join")]
    public async Task<IActionResult> Join(JoinBoardDto dto)
    {
        var board=await _boards.Join(dto.Code, UserId);
        if(board==null) return NotFound("Board not found");
        return Ok(new BoardResponseDto(board.Id, board.Name, board.Code));
    }
}