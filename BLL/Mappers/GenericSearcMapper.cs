using AutoMapper;
using DTO.Search;
using ENTITIES.Entities;

namespace BLL.Mappers;
public class GenericSearcMapper : Profile
{
    public GenericSearcMapper()
    {
        CreateMap<BuildingType, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdBuildingType))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BuildingTypeName));
        CreateMap<Document, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdDocument))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DocumentName));
        CreateMap<OperationType, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdOperationType))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OperationTypeName));
        CreateMap<OwnerType, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdOwnerType))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OwnerTypeName));
        CreateMap<PropertyType, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPropertyType))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PropertyTypeName));
        CreateMap<Region, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdRegion))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RegionName))
            .ForMember(dest => dest.Keyword, opt => opt.MapFrom(src => src.Keyword01));
        CreateMap<RegionUnit01, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdRegion))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RegionName));
        CreateMap<RepairRate, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdRepairRate))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RepairRateName));
        CreateMap<RoomCount, FilterItemDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdRoomCount))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoomCountName));

        CreateMap<Currency, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCurrency))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Currency1));
        CreateMap<Target, FilterItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdTarget))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TargetName));
    }
}
