using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.DTOs.User;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Admin.Users.Query.GetAllVendorsByAdminQuery;
public class GetAllVendorsByAdminQuery : IRequest<ApiResponse<List<UserDTO>>>
{

}
