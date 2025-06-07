using Cloud_Mall.Application.DTOs.StoreCategory;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.StoreCategory.Query
{
    internal class StoreCategoryQueryHandler : IRequestHandler<StoreCategoryQuery, List<StoreCategoryDto>>
    {
        private readonly IStoreCategoryRepository repository;

        public StoreCategoryQueryHandler(IStoreCategoryRepository _repository)
        {
            repository = _repository;
        }
        public async Task<List<StoreCategoryDto>> Handle(StoreCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await repository.GetAllAsync();
            var categoryDtos = categories.Select(c => new StoreCategoryDto
            {
                Id = c.ID,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            return categoryDtos;
        }
    }
}
