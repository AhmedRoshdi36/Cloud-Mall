using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.DTOs.Vendor;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Admin.Vendor.Query.GetAllVendorsQuery;

public class GetAllVendorsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllVendorsByAdminQuery, ApiResponse<List<VendorDTO>>>
{
   
     private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<ApiResponse<List<VendorDTO>>> Handle(GetAllVendorsByAdminQuery request, CancellationToken cancellationToken)
    {
    
        var vendors = await unitOfWork.VendorRepository.GetAllVendorsAsync();
        
        if (vendors == null)
        {
            return ApiResponse<List<VendorDTO>>.Failure("There is no vendors");
        }
        var vendorsDto = vendors.Select(u => new VendorDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
        }).ToList();
       

        return ApiResponse<List<VendorDTO>>.SuccessResult(vendorsDto);
    }
   
}
    
