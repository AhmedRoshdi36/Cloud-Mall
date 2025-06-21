using AutoMapper;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllStoresQuery;
public class GetAllStoresQueryHandler(IStoreRepository storeRepository, IMapper mapper)
    : IRequestHandler<GetAllStoresQuery, List<GetAllStoresDTO>>
{
    public async Task<List<GetAllStoresDTO>> Handle(GetAllStoresQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Store> stores;
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            stores = await storeRepository.GetStoresByCategoryNameAsync(request.CategoryName);
        }
        else
        {
            stores = await storeRepository.GetAllAsync();
        }
        stores = stores.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
        return mapper.Map<List<GetAllStoresDTO>>(stores);
    }
}