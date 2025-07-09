using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories.DeliveryCompanyRepository
{
    public class DeliveryCompanyRepository : IDeliveryCompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryCompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryCompany> CreateAsync(DeliveryCompany deliveryCompany)
        {
            await _context.DeliveryCompanies.AddAsync(deliveryCompany);
            await _context.SaveChangesAsync();
            return deliveryCompany;
        }

        public async Task<DeliveryCompany> GetByIdAsync(int id)
        {
            return await _context.DeliveryCompanies
                .Include(dc => dc.User)
                .FirstOrDefaultAsync(dc => dc.ID == id);
        }

        public async Task<DeliveryCompany> GetByCommercialSerialAsync(string commercialSerial)
        {
            return await _context.DeliveryCompanies
                .Include(dc => dc.User)
                .FirstOrDefaultAsync(dc => dc.CommercialSerialNumber == commercialSerial);
        }

        public async Task<DeliveryCompany> GetByUserIdAsync(string userId)
        {
            return await _context.DeliveryCompanies
                .Include(dc => dc.User)
                .FirstOrDefaultAsync(dc => dc.UserID == userId);
        }

        public async Task<IEnumerable<DeliveryCompany>> GetAllAsync()
        {
            return await _context.DeliveryCompanies
                .Include(dc => dc.User)
                .ToListAsync();
        }

        public async Task<bool> CommercialSerialExistsAsync(string commercialSerial)
        {
            return await _context.DeliveryCompanies
                .AnyAsync(dc => dc.CommercialSerialNumber == commercialSerial);
        }
    }
} 