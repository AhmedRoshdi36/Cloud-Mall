
using Cloud_Mall.Application.DTOs.User;
using MediatR;

namespace Cloud_Mall.Application.Admin.Users.Query.GetAllClientsByAdminQuery;

public class GetAllClientsByAdminQuery :IRequest<ApiResponse<List<UserDTO>>>
{
}
