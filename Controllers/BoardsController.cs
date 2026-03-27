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
    [HttpPost]
    public async Task<IActionResult> Create(CreateBoardDto dto)
    {
        var board=await _boards.Create(dto.Name, UserId);
        return Ok(new BoardResponseDto(board.Id, board.Name, board.Code));
    }
    [HttpPost("join")]
    public async Task<IActionResult> Join(JoinBoardDto dto)
    {
        var board=await _boards.Join(dto.Code, UserId);
        if(board==null) return NotFound("Board not found");
        return Ok(new BoardResponseDto(board.Id, board.Name, board.Code));
    }
}