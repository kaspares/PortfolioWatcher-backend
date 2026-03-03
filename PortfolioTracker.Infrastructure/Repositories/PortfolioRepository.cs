using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Domain.Repositories;
using PortfolioTracker.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Infrastructure.Repositories
{
    internal class PortfolioRepository(PortfolioTrackerDbContext dbContext) : IPortfolioRepository
    {
        public async Task AddAsync(Portfolio portfolio)
        {
            await dbContext.AddAsync(portfolio);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Portfolio portfolio)
        {
            dbContext.Portfolios.Remove(portfolio);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Portfolio>> GetAllByUserIdAsync(string userId)
        {
            return await dbContext.Portfolios
                .Include(p => p.Items)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Portfolio?> GetByIdAsync(Guid id)
        {
            return await dbContext.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Portfolio?> GetByIdWithItemsAsync(Guid id)
        {
            return await dbContext.Portfolios
                .Include(p =>p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Portfolio portfolio)
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
