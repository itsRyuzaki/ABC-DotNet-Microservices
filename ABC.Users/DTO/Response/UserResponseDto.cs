namespace ABC.Users.DTO.Response;

public class UserResponseDTO : ApiResponseDto
{

    public required string FirstName { get; set; }

    public string? AvatarUrl { get; set; }

    public string[] AccessibleModules { get; set; } = [];

}