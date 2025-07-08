
using AutoMapper;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Admin.StoreManagement.Query.GetAllStoresByAdmin;

public class GetAllStoresByAdminQueryHandler(IStoreRepository storeRepository, IMapper mapper)
    : IRequestHandler<GetAllStoresByAdminQuery, GetAllStoresByAdminWithPaginationDTO>
{
    public async Task<GetAllStoresByAdminWithPaginationDTO> Handle(GetAllStoresByAdminQuery request, CancellationToken cancellationToken)
    {
        List<Store> stores;
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            stores = await storeRepository.GetStoresByCategoryNameByAdminAsync(request.CategoryName);
        }
        else
        {
            stores = await storeRepository.GetAllForAdminAsync();
        }

        int storeCount = stores.Count;
        stores = stores.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
        return new GetAllStoresByAdminWithPaginationDTO()
        {
            PageSize = request.PageSize,
            TotalCount = storeCount,
            CurrentPage = request.PageNumber,
            TotalNumberOfPages = (int)Math.Ceiling((decimal)storeCount / (decimal)request.PageSize),
            AllStores = mapper.Map<List<GetAllStoresByAdminDTO>>(stores)
        };
    }

 
}