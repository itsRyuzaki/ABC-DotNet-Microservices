using ABC.Users.DTO.Request;
using ABC.Users.DTO.Response;

namespace ABC.Users.Facade;

public interface IUserFacade {

    public Task<ApiResponseDto> SignUpUserAsync(UserSignUpDto signUpRequest);

    public Task<UserResponseDTO> LoginUserAsync(UserLoginDto loginRequest);

    public Task<string> CreateSessionHistory(string userName);


}