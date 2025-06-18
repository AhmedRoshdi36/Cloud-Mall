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
        var stores = await storeRepository.GetAllAsync();
        return mapper.Map<List<GetAllStoresDTO>>(stores);
    }
}