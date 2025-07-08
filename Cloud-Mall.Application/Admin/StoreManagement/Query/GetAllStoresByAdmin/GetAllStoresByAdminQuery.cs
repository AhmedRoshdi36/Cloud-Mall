using Cloud_Mall.Application.DTOs.Store;
using MediatR;

namespace Cloud_Mall.Application.Admin.StoreManagement.Query.GetAllStoresByAdmin;

public class GetAllStoresByAdminQuery : IRequest<GetAllStoresByAdminWithPaginationDTO>
{
    public string? CategoryName { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public GetAllStoresByAdminQuery(string? categoryName = null, int pageNumber = 1, int pageSize = 10)
    {
        CategoryName = categoryName;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

}
