using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Domain.Interfaces;
using PortfolioTracker.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Application.Services
{
    public class PortfolioItemService(ILogger<PortfolioItemService> logger,
        IMapper mapper,
        IPortfolioItemRepository portfolioItemRepository,
        IPortfolioRepository portfolioRepository,
        ICurrentUserService currentUser) : IPortfolioItemService
    {
        private async Task GetAuhtorizedAsync(Guid portfolioid)
        {
            var portfolio = await portfolioRepository.GetByIdAsync(portfolioid)
              ?? throw new Exception("Portfolio not found");

            if (portfolio.UserId != currentUser.userId)
                throw new Exception("Forbidden");

            return;
        }
        public async Task CreateAsync(CreatePortfolioItemDto dto, Guid portfolioId)
        {
            logger.LogInformation("Creating new portfolio item {@dto} in portfolio with id: {@PortfolioId}", dto, portfolioId);
            
            await GetAuhtorizedAsync(portfolioId);

            var portfolioItem = mapper.Map<PortfolioItem>(dto);
            portfolioItem.PortfolioId = portfolioId;

            await portfolioItemRepository.AddAsync(portfolioItem);

        }

        public async Task DeleteAsync(Guid portfolioId)
        {
            logger.LogInformation("Deleting portfolio with id: {@portfolioId}", portfolioId);
            await GetAuhtorizedAsync(portfolioId);

            var portfolio = await portfolioItemRepository.GetPortfolioItemByIdAsync(portfolioId) 
                ?? throw new Exception("PortfolioItem not found");

            await portfolioItemRepository.DeleteAsync(portfolio);
        }

        public async Task<IEnumerable<PortfolioItemDto>> GetAllByPortfolio(Guid portfolioId)
        {
            logger.LogInformation("Getting all portfolioItems for portfolio with id: {@portfolioId}", portfolioId);
            await GetAuhtorizedAsync(portfolioId);
            var portfolios = await portfolioItemRepository.GetAllPortfolioItemInPortfolio(portfolioId);

            return mapper.Map<IEnumerable<PortfolioItemDto>>(portfolios);

        }

        public async Task<PortfolioItemDto> GetByIdAsync(Guid id)
        {
            logger.LogInformation("Getting portfolioItem with id: {@id}", id);
            await GetAuhtorizedAsync(id);
            var portfolioItem = await portfolioItemRepository.GetPortfolioItemByIdAsync(id)
                ?? throw new Exception("Portfolio item not found");

            return mapper.Map<PortfolioItemDto>(portfolioItem);
        }

        public Task UpdateAsync(UpdatePortfolioItemDto dto, Guid portfolioId)
        {
            throw new NotImplementedException();
        }
    }
}
