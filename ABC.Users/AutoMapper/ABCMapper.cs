using ABC.Users.DTO;
using ABC.Users.Models;
using AutoMapper;

namespace ABC.Users.ABCMapper;

public class ABCMapper: Profile {
    public ABCMapper() {
        CreateMap<UserSignUpDto, User>();
    }
}