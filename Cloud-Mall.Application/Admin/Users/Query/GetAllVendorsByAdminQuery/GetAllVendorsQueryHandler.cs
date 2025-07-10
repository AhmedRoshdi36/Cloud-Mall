using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.DTOs.User;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Admin.Users.Query.GetAllVendorsByAdminQuery;

public class GetAllVendorsQueryHandler(IIdentityRepository identityRepository) 
    : IRequestHandler<GetAllVendorsByAdminQuery, ApiResponse<List<UserDTO>>>
{
   

    public async Task<ApiResponse<List<UserDTO>>> Handle(GetAllVendorsByAdminQuery request, CancellationToken cancellationToken)
    {
    
        var vendors = await identityRepository.GetUsersInRoleAsync("Vendor");

        if (vendors == null)
        {
            return ApiResponse<List<UserDTO>>.Failure("There is no vendors");
        }
        var vendorsDto = vendors.Select(u => new UserDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            Role = "Vendor"

        }).ToList();
       

        return ApiResponse<List<UserDTO>>.SuccessResult(vendorsDto);
    }
   
}
    
