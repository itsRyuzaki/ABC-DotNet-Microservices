using ABC.Users.DTO;
using ABC.Users.Models;

namespace ABC.Users.Services;

public interface IUserService
{
    public Task<ApiResponseDto> InsertUserDataAsync(User userData);
    public  Task<ApiResponseDto> InsertUserAuthDataAsync(UserAuth authData);
    public  Task<UserAuth> GetUserAuthAsync(string userName);
}