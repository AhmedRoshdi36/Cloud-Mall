using Cloud_Mall.Application.DTOs.User;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.Users.Query.GetAllAdminsBySuperAdminQuery;

//internal class GetAllAdminsBySuperAdminQueryHandler
//{
//}

public class GetAllAdminsBySuperAdminQueryHandler(IIdentityRepository identityRepository)
    : IRequestHandler<GetAllAdminsBySuperAdminQuery, ApiResponse<List<UserDTO>>>
{

    private readonly IIdentityRepository identityRepository = identityRepository;

    public async Task<ApiResponse<List<UserDTO>>> 
        Handle(GetAllAdminsBySuperAdminQuery request, CancellationToken cancellationToken)
    {

        //var vendors = await unitOfWork.UserRepository.GetAllVendorsAsync();
        var Admins = await identityRepository.GetUsersInRoleAsync("Admin");

        if (Admins == null)
        {
            return ApiResponse<List<UserDTO>>.Failure("There is no Admins");
        }
        var AdminsDto = Admins.Select(u => new UserDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            Role = "Admin"
        }).ToList();


        return ApiResponse<List<UserDTO>>.SuccessResult(AdminsDto);
    }

}
