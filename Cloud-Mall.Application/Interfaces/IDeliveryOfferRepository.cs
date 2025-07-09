using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Domain.Enums;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IDeliveryOfferRepository
    {
        Task<Domain.Entities.DeliveryOffer> CreateAsync(Domain.Entities.DeliveryOffer deliveryOffer);
        Task<Domain.Entities.DeliveryOffer> GetByIdAsync(int id);
        Task<IEnumerable<Domain.Entities.DeliveryOffer>> GetByCustomerOrderIdAsync(int customerOrderId);
        Task<IEnumerable<Domain.Entities.DeliveryOffer>> GetByDeliveryCompanyIdAsync(int deliveryCompanyId);
        Task<IEnumerable<Domain.Entities.DeliveryOffer>> GetPendingOffersByCustomerOrderIdAsync(int customerOrderId);
        Task UpdateAsync(Domain.Entities.DeliveryOffer deliveryOffer);
        Task UpdateMultipleAsync(IEnumerable<Domain.Entities.DeliveryOffer> deliveryOffers);
    }
} 