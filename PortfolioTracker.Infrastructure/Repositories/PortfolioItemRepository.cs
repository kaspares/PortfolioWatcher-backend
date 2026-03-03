using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Domain.Interfaces;
using PortfolioTracker.Infrastructure.Persistence;

namespace PortfolioTracker.Infrastructure.Repositories
{
    internal class PortfolioItemRepository(PortfolioTrackerDbContext dbContext) : IPortfolioItemRepository
    {
        public async Task AddAsync(PortfolioItem item)
        {
            await dbContext.PortfolioItems.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(PortfolioItem item)
        {
            dbContext.PortfolioItems.Remove(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PortfolioItem?> GetPortfolioItemByIdAsync(Guid id)
        {
            return await dbContext.PortfolioItems.FirstOrDefaultAsync(pi => pi.Id == id);

        }

        public async Task UpdateAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
