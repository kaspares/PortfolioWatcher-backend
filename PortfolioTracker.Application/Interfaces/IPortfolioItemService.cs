using PortfolioTracker.Application.DTOs;

namespace PortfolioTracker.Application.Interfaces;

public interface IPortfolioItemService
{
    Task<IEnumerable<PortfolioItemDto>> GetAllByPortfolio(Guid portfolioId);
    Task<PortfolioItemDto> GetByIdAsync(Guid id);
    Task CreateAsync(CreatePortfolioItemDto dto, Guid portfolioId);
    Task UpdateAsync(UpdatePortfolioItemDto dto, Guid portfolioId);
    Task DeleteAsync(Guid portfolioId);
}
