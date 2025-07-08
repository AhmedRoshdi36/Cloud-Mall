using AutoMapper;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery;
using Cloud_Mall.Domain.Entities;
namespace Cloud_Mall.Application.Stores.Profiler;
public class Profiler : Profile
{
    public Profiler()
    {
        CreateMap<Domain.Entities.Store, DTOs.Store.GetAllStoresDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.StoreCategory.Name))
            .ForMember(dest => dest.VendorID, opt => opt.MapFrom(src => src.VendorID));
        CreateMap<Domain.Entities.Store, DTOs.Store.GetOneStoreDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.StoreCategory.Name));
        //.ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));

        CreateMap<Store, GetAllStoresByAdminDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.StoreCategory.Name))
            .ForMember(dest => dest.VendorID, opt => opt.MapFrom(src => src.VendorID))
            .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.Name))
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));

        CreateMap<StoreAddress, GetStoreAddressDTO>();

    }
}
