using AutoMapper;
using System.Linq;
namespace Cloud_Mall.Application.Products.Profiler;
public class Profiler : Profile
{
    public Profiler()
    {
        CreateMap<Domain.Entities.Product, DTOs.Product.GetAllProductsDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ProductCategory.Name))
            .ForMember(dest => dest.AverageRate, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rate) : 0));

        CreateMap<Domain.Entities.Product, DTOs.Product.GetProductByIdDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ProductCategory.Name))
            .ForMember(dest => dest.AverageRate, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rate) : 0));
    }
} 