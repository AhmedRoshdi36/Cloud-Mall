using Cloud_Mall.Application.DTOs.GoverningLocation;
using MediatR;

namespace Cloud_Mall.Application.GoverningLocation.Query.GetAllGoverningLocations
{
    public class GetAllGoverningLocationsQuery : IRequest<List<GoverningLocationDTO>>
    {
    }
}
