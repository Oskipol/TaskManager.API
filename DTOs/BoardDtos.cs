public record CreateBoardDto(string Name);
public record JoinBoardDto(string Code);
public record BoardResponseDto(int Id, string Name, string Code);
