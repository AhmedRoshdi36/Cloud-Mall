using AutoMapper;
using Cloud_Mall.Application.DTOs.Product;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Products.Query.GetAllProductsQuery;
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, GetAllProductsWithPaginationDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<GetAllProductsWithPaginationDTO> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.GetAllProductsAsync(
            request.StoreId,
            request.Name,
            request.Brand,
            request.MinPrice,
            request.MaxPrice,
            request.MinRate,
            request.MaxRate,
            request.Category,
            request.PageNumber,
            request.PageSize);
        return new GetAllProductsWithPaginationDTO()
        {
            PageSize = request.PageSize,
            TotalCount = totalCount,
            CurrentPage = request.PageNumber,
            TotalNumberOfPages = (int)Math.Ceiling((decimal)totalCount / request.PageSize),
            AllProducts = _mapper.Map<List<GetAllProductsDTO>>(products)
        };
    }
}