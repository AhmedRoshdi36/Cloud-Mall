using Cloud_Mall.Application.DTOs.Store;
using MediatR;

public class GetAllStoresWithFilterationQuery : IRequest<ApiResponse<GetAllStoresWithPaginationDTO>>
{
    public int? CategoryId { get; set; }
    public int? GoverningLocationId { get; set; }
    public string? StreetAddress { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
