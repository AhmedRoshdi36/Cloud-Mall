

using Cloud_Mall.Application.DTOs.User;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Admin.Users.Query.GetAllClientsByAdminQuery;

internal class GetAllClientsByAdminQueryHandler(IIdentityRepository identityRepository)
    : IRequestHandler<GetAllClientsByAdminQuery, ApiResponse<List<UserDTO>>>
{

    private readonly IIdentityRepository identityRepository = identityRepository;

    public async Task<ApiResponse<List<UserDTO>>>
        Handle(GetAllClientsByAdminQuery request, CancellationToken cancellationToken)
    {

        var Clients = await identityRepository.GetUsersInRoleAsync("Client");

        if (Clients == null)
        {
            return ApiResponse<List<UserDTO>>.Failure("There is no Clients");
        }
        var ClientsDto = Clients.Select(u => new UserDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            Role = "Client"
        }).ToList();


        return ApiResponse<List<UserDTO>>.SuccessResult(ClientsDto);
    }

}

