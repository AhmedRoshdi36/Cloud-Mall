using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;

namespace Cloud_Mall.Application.Stores.Query.GetStoreByIdQuery
{
    public class GetStoreByIdQueryHandler : IRequestHandler<GetStoreByIdQuery, GetOneStoreDTO>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IMapper _mapper;

        public GetStoreByIdQueryHandler(IStoreRepository storeRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _mapper = mapper;
        }

        public async Task<GetOneStoreDTO> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
        {
            var store = await _storeRepository.GetStoreByIdAsync(request.StoreId);
            if (store == null)
                return null;
            return _mapper.Map<GetOneStoreDTO>(store);
        }
    }
} 