using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Stores.Command.DeleteStore
{
    public class DeleteStoreCommandHandler : IRequestHandler<DeleteStoreCommand, ApiResponse<bool>>
    {
        private readonly IFileService fileService;
        private readonly IUnitOfWork unitOfWork;
        public DeleteStoreCommandHandler()
        {

        }
        public Task<ApiResponse<bool>> Handle(DeleteStoreCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
