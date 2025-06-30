using AutoMapper;
using Cloud_Mall.Application.DTOs.Product;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Products.Query.GetAllProductsQuery;
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<GetAllProductsDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<List<GetAllProductsDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsAsync(
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
        return _mapper.Map<List<GetAllProductsDTO>>(products);
    }
} 