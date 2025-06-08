using Cloud_Mall.Application.DTOs.GoverningLocation;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.GoverningLocation.Query.GetAllGoverningLocations
{
    public class GetAllGoverningLocationsQueryhandler : IRequestHandler<GetAllGoverningLocationsQuery, List<GoverningLocationDTO>>
    {
        private readonly IGoverningLocationRepository repository;

        public GetAllGoverningLocationsQueryhandler(IGoverningLocationRepository _repository)
        {
            repository = _repository;
        }

        public async Task<List<GoverningLocationDTO>> Handle(GetAllGoverningLocationsQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetAllAsync();
        }
    }
}
