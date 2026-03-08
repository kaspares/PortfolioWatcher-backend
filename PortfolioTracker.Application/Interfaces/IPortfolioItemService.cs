using PortfolioTracker.Application.DTOs;

namespace PortfolioTracker.Application.Interfaces;

public interface IPortfolioItemService
{
    Task<IEnumerable<PortfolioItemDto>> GetAllByPortfolio(Guid portfolioId);
    Task<PortfolioItemDto> GetByIdAsync(Guid portfolioId, Guid portfolioItemId);
    Task CreateAsync(CreatePortfolioItemDto dto, Guid portfolioId);
    Task UpdateAsync(UpdatePortfolioItemDto dto, Guid portfolioId, Guid portfolioItemId);
    Task DeleteAsync(Guid portfolioId, Guid portfolioItemId);
}
