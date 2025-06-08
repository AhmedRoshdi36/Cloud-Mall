using Cloud_Mall.Application.DTOs.GoverningLocation;
using MediatR;

namespace Cloud_Mall.Application.GoverningLocation.Command.CreateGoverningLocation
{
    public class CreateGoverningLocationCommand : IRequest<GoverningLocationDTO>
    {
        public string Name { get; set; }
        public string Region { get; set; }
    }
}
