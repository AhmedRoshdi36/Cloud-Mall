using AutoMapper;
using Cloud_Mall.Application.DTOs.Product;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Products.Query.GetProductByIdQuery;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<GetProductByIdDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.ProductId);
        if (product == null)
            return null;
        return _mapper.Map<GetProductByIdDTO>(product);
    }
} 