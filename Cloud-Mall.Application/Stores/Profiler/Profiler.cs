using AutoMapper;
namespace Cloud_Mall.Application.Stores.Profiler;
public class Profiler : Profile
{
    public Profiler()
    {
        CreateMap<Domain.Entities.Store, DTOs.Store.GetAllStoresDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.StoreCategory.Name));
    }
}
