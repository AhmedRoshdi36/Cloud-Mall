using Cloud_Mall.Application.DTOs.Store;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllStoresQuery;
public class GetAllStoresQuery : IRequest<List<GetAllStoresDTO>>
{
}