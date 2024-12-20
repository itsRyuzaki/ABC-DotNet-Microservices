namespace ABC.Users.DTO;

public class UserSignUpDto
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string FirstName { get; set; }

    public string? LastName { get; set; }

    public required string EmailId { get; set; }

    public required string MobileNumber { get; set; }

}