using Cloud_Mall.Application.DTOs.GoverningLocation;
using MediatR;

namespace Cloud_Mall.Application.GoverningLocations.Query.GetAllGoverningLocations
{
    public class GetAllGoverningLocationsQuery : IRequest<List<GoverningLocationDTO>>
    {
    }
}
