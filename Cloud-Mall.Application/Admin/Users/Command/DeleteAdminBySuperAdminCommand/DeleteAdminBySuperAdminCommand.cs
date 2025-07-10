

using Cloud_Mall.Application.DTOs.Auth;
using MediatR;

namespace Cloud_Mall.Application.Admin.Users.Command.DeleteAdminBySuperAdminCommand;

public class DeleteAdminBySuperAdminCommand(string AdminId) : IRequest<ApiResponse<bool>>
{
    public string AdminId { get; set; } = AdminId;
}
