using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IDeliveryCompanyRepository
    {
        Task<Domain.Entities.DeliveryCompany> CreateAsync(Domain.Entities.DeliveryCompany deliveryCompany);
        Task<Domain.Entities.DeliveryCompany> GetByIdAsync(int id);
        Task<Domain.Entities.DeliveryCompany> GetByCommercialSerialAsync(string commercialSerial);
        Task<Domain.Entities.DeliveryCompany> GetByUserIdAsync(string userId);
        Task<IEnumerable<Domain.Entities.DeliveryCompany>> GetAllAsync();
        Task<bool> CommercialSerialExistsAsync(string commercialSerial);
    }
} 