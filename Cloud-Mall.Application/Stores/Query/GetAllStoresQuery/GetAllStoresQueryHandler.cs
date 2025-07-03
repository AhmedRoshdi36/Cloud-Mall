using AutoMapper;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllStoresQuery;
public class GetAllStoresQueryHandler(IStoreRepository storeRepository, IMapper mapper)
    : IRequestHandler<GetAllStoresQuery, GetAllStoresWithPaginationDTO>
{
    public async Task<GetAllStoresWithPaginationDTO> Handle(GetAllStoresQuery request, CancellationToken cancellationToken)
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
        int storeCount = stores.Count;
        stores = stores.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
        return new GetAllStoresWithPaginationDTO()
        {
            PageSize = request.PageSize,
            TotalCount = storeCount,
            CurrentPage = request.PageNumber,
            TotalNumberOfPages = (int)Math.Ceiling((decimal)storeCount / (decimal)request.PageSize),
            AllStores = mapper.Map<List<GetAllStoresDTO>>(stores)
        };
    }
}