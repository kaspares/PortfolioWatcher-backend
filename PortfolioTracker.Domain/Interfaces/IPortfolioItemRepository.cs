using PortfolioTracker.Domain.Entities;

namespace PortfolioTracker.Domain.Interfaces;

public interface IPortfolioItemRepository
{
    Task<IEnumerable<PortfolioItem>> GetAllPortfolioItemInPortfolio(Guid id);
    Task<PortfolioItem> GetPortfolioItemByIdAsync(Guid id);
    Task AddAsync(PortfolioItem item);
    Task DeleteAsync(PortfolioItem item);
    Task UpdateAsync();
}
