using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllStoresWithFilteration
{
    internal class GetAllStoresWithFilterationQueryHandler : IRequestHandler<GetAllStoresWithFilterationQuery, ApiResponse<GetAllStoresWithPaginationDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllStoresWithFilterationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<GetAllStoresWithPaginationDTO>> Handle(GetAllStoresWithFilterationQuery request, CancellationToken cancellationToken)
        {
            var (stores, totalCount) = await _unitOfWork.StoresRepository.GetFilteredStoresAsync(
                request.CategoryId,
                request.GoverningLocationId,
                request.StreetAddress,
                request.PageNumber,
                request.PageSize);

            var storeDtos = stores.Select(s => new GetAllStoresDTO
            {
                ID = s.ID,
                VendorID = s.VendorID,
                Name = s.Name,
                LogoURL = s.LogoURL,
                Description = s.Description,
                CategoryName = s.StoreCategory.Name
            }).ToList();

            var responseDto = new GetAllStoresWithPaginationDTO
            {
                PageSize = request.PageSize,
                TotalCount = totalCount,
                CurrentPage = request.PageNumber,
                TotalNumberOfPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                AllStores = storeDtos
            };

            return ApiResponse<GetAllStoresWithPaginationDTO>.SuccessResult(responseDto);
        }

    }
}
