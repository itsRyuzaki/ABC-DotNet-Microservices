using AutoMapper;
using ABC.Accessories.DTO.Request;
using ABC.Accessories.Models;

namespace ABC.Accessories.AutoMapper;

public class AccessoriesMapper : Profile
{
    public AccessoriesMapper()
    {
        CreateMap<AddAccessoriesDTO, Accessory>();
    }
}