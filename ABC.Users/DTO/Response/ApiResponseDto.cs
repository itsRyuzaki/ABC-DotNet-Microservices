namespace ABC.Users.DTO.Response;


public class ErrorDetails
{
    public required int Code { get; set; }
    public string[] Details { get; set; } = [];
}
public class ApiResponseDto
{
    public bool Success { get; set; }

    public ErrorDetails? ErrorDetails {get; set;}
    public static ApiResponseDto HandleErrorResponse(int code, string[] details)
    {
        return new ApiResponseDto()
        {
            Success = false,
            ErrorDetails = new ErrorDetails()
            {
                Code = code,
                Details = details
            }
        };
    }
}