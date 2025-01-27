using AutoMapper;
using ABC.Accessories.DTO.Request;
using ABC.Accessories.Models;

namespace ABC.Accessories.AutoMapper;

public class AccessoriesMapper : Profile
{
    public AccessoriesMapper()
    {
        CreateMap<AddAccessoryDTO, Accessory>();
        CreateMap<AddAccessoryBaseDTO, AccessoryBase>();
        CreateMap<ItemImageDTO, ItemImage>();
        CreateMap<AddSellerDTO, Seller>();
    }
}