using PortfolioTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Domain.Repositories
{
    public interface IPortfolioRepository
    {
        Task<Portfolio?> GetByIdAsync(Guid id);
        Task<Portfolio?> GetByIdWithItemsAsync(Guid id);
        Task<IEnumerable<Portfolio>> GetAllByUserIdAsync(string userId);
        Task AddAsync(Portfolio portfolio);
        Task UpdateAsync(Portfolio portfolio);
        Task DeleteAsync(Portfolio portfolio);
    }
}
