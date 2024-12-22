using ABC.Users.DTO;

namespace ABC.Users.Facade;

public interface IUserFacade {

    public Task<ApiResponseDto> SignUpUserAsync(UserSignUpDto signUpRequest);

    public Task<ApiResponseDto> LoginUserAsync(UserLoginDto loginRequest);

}