using Cloud_Mall.Application.DTOs.GoverningLocation;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.GoverningLocation.Command.CreateGoverningLocation
{
    internal class CreateGoverningLocationCommandHandler : IRequestHandler<CreateGoverningLocationCommand, GoverningLocationDTO>
    {
        private readonly IGoverningLocationRepository repository;

        public CreateGoverningLocationCommandHandler(IGoverningLocationRepository _repository)
        {
            repository = _repository;
        }

        public async Task<GoverningLocationDTO> Handle(CreateGoverningLocationCommand request, CancellationToken cancellationToken)
        {
            return await repository.CreateAsync(request.Name, request.Region);
        }
    }
}
