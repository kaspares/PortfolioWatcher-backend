using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Application.Interfaces
{
    public interface IPortfolioService
    {
        Task<IEnumerable<PortfolioSummaryDto>> GetUserPortfoliosAsync();
        Task<PortfolioDetailDto> GetByIdWithItemsAsync(Guid id);
        Task CreateAsync(CreatePortfolioDto portfolio);
        Task UpdateAsync(UpdatePortfolioDto portfolio, Guid id);
        Task DeleteAsync(Guid id);
    }
}
