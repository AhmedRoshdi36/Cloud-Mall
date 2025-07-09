using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Domain.Enums;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories.DeliveryOfferRepository
{
    public class DeliveryOfferRepository : IDeliveryOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryOfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryOffer> CreateAsync(DeliveryOffer deliveryOffer)
        {
            await _context.DeliveryOffers.AddAsync(deliveryOffer);
            await _context.SaveChangesAsync();
            return deliveryOffer;
        }

        public async Task<DeliveryOffer> GetByIdAsync(int id)
        {
            return await _context.DeliveryOffers
                .Include(offer => offer.DeliveryCompany)
                .Include(offer => offer.CustomerOrder)
                .FirstOrDefaultAsync(offer => offer.ID == id);
        }

        public async Task<IEnumerable<DeliveryOffer>> GetByCustomerOrderIdAsync(int customerOrderId)
        {
            return await _context.DeliveryOffers
                .Include(offer => offer.DeliveryCompany)
                .Include(offer => offer.CustomerOrder)
                .Where(offer => offer.CustomerOrderID == customerOrderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryOffer>> GetByDeliveryCompanyIdAsync(int deliveryCompanyId)
        {
            return await _context.DeliveryOffers
                .Include(offer => offer.DeliveryCompany)
                .Include(offer => offer.CustomerOrder)
                .Where(offer => offer.DeliveryCompanyID == deliveryCompanyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryOffer>> GetPendingOffersByCustomerOrderIdAsync(int customerOrderId)
        {
            return await _context.DeliveryOffers
                .Include(offer => offer.DeliveryCompany)
                .Include(offer => offer.CustomerOrder)
                .Where(offer => offer.CustomerOrderID == customerOrderId && offer.Status == DeliveryOfferStatus.Pending)
                .ToListAsync();
        }

        public async Task UpdateAsync(DeliveryOffer deliveryOffer)
        {
            _context.DeliveryOffers.Update(deliveryOffer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMultipleAsync(IEnumerable<DeliveryOffer> deliveryOffers)
        {
            _context.DeliveryOffers.UpdateRange(deliveryOffers);
            await _context.SaveChangesAsync();
        }
    }
} 