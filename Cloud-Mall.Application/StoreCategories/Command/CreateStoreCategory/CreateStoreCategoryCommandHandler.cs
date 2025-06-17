using Cloud_Mall.Application.DTOs.StoreCategory;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.StoreCategories.Command.CreateStoreCategory
{
    public class CreateStoreCategoryCommandHandler : IRequestHandler<CreateStoreCategoryCommand, StoreCategoryDto>
    {
        private readonly IStoreCategoryRepository repository;

        public CreateStoreCategoryCommandHandler(IStoreCategoryRepository _repository)
        {
            this.repository = _repository;
        }

        public async Task<StoreCategoryDto> Handle(CreateStoreCategoryCommand request, CancellationToken cancellationToken)
        {
            return await repository.CreateAsync(request.Name, request.Description);
        }
    }
}
