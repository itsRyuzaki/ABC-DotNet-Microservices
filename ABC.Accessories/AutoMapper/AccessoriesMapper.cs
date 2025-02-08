using AutoMapper;
using ABC.Accessories.DTO.Request;
using ABC.Accessories.Models;
using ABC.Accessories.Models.MongoDb;

namespace ABC.Accessories.AutoMapper;

public class AccessoriesMapper : Profile
{
    public AccessoriesMapper()
    {
        CreateMap<AddAccessoryDTO, Accessory>();
        CreateMap<AddAccessoryDTO, AccessoryExtras>();
        CreateMap<AddAccessoryBaseDTO, AccessoryBase>();
        CreateMap<AddAccessoryBaseDTO, AccessoryBaseExtras>();
        CreateMap<ItemImageDTO, ItemImage>();
        CreateMap<AddSellerDTO, Seller>();
    }
}