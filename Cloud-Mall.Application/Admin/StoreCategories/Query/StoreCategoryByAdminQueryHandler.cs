using Cloud_Mall.Application.DTOs.StoreCategory;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.StoreCategories.Query
{
    internal class StoreCategoryByAdminQueryHandler : IRequestHandler<StoreCategoryByAdminQuery, List<StoreCategoryDto>>
    {
        private readonly IStoreCategoryRepository repository;

        public StoreCategoryByAdminQueryHandler(IStoreCategoryRepository _repository)
        {
            repository = _repository;
        }
        public async Task<List<StoreCategoryDto>> Handle(StoreCategoryByAdminQuery request, CancellationToken cancellationToken)
        {
            var categories = await repository.GetAllAsync();
            return categories;
        }
    }
}
