using ABC.Users.DTO.Request;
using ABC.Users.DTO.Response;

namespace ABC.Users.Facade;

public interface IUserFacade
{

    public Task<ApiResponseDto<string>> SignUpUserAsync(UserSignUpDto signUpRequest);

    public Task<ApiResponseDto<UserResponseDTO>> LoginUserAsync(UserLoginDto loginRequest);

    public Task<string?> CreateSessionHistoryAsync(string userName);

    public Task<ApiResponseDto<UserResponseDTO>> GetUserDetailsFromSessionAsync(string sessionToken);


}