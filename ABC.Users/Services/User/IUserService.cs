using ABC.Users.DTO;

namespace ABC.Users.Services;

public interface IUserService
{
    public Task AddUserAsync(UserSignUpDto signUpRequest);
}