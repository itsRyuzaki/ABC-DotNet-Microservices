namespace ABC.Users.DTO;


public class ErrorDetails
{
    public required int Code { get; set; }
    public string[] Details { get; set; } = [];
}
public class ApiResponseDto
{
    public bool Success { get; set; }

    public ErrorDetails? ErrorDetails {get; set;}
}