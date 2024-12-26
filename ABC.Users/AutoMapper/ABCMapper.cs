using ABC.Users.DTO.Request;
using ABC.Users.DTO.Response;
using ABC.Users.Models;
using AutoMapper;

namespace ABC.Users.ABCMapper;

public class ABCMapper : Profile
{
    public ABCMapper()
    {
        CreateMap<UserSignUpDto, User>();
        CreateMap<ApiResponseDto, UserResponseDTO>();
        CreateMap<User, UserResponseDTO>();

    }
}