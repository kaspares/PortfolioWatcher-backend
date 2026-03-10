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
        ICurrentUserService currentUser,
        IMarketDataProvider marketDataProvider) : IPortfolioItemService
    {

        private async Task EnrichItemWithMarketDataAsync(PortfolioItemDto item)
        {
            try
            {
                var price = await marketDataProvider.GetCurrentPriceAsync(item.Ticker);
                item.CurrentPrice = price;
                item.MarketValue = item.Quantity * price;
                var cost = item.Quantity * item.PurchasePrice;
                item.ProfitLoss = item.MarketValue - cost;
                item.ProfitLossPercent = cost != 0 ? Math.Round(item.ProfitLoss.Value / cost * 100, 2) : 0;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to fetch price for {Ticker}", item.Ticker);
            }
        }

        private async Task GetAuhtorizedAsync(Guid portfolioId)
        {
            var portfolio = await portfolioRepository.GetByIdAsync(portfolioId)
              ?? throw new NotImplementedException("Portfolio not found");

            if (portfolio.UserId != currentUser.userId)
                throw new NotImplementedException("Forbidden");

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

        public async Task DeleteAsync(Guid portfolioId, Guid portfolioItemId)
        {
            logger.LogInformation("Deleting portfolio with id: {@portfolioId}", portfolioId);
            await GetAuhtorizedAsync(portfolioId);

            var portfolio = await portfolioItemRepository.GetPortfolioItemByIdAsync(portfolioId) 
                ?? throw new NotImplementedException("PortfolioItem not found");

            await portfolioItemRepository.DeleteAsync(portfolio);
        }

        public async Task<IEnumerable<PortfolioItemDto>> GetAllByPortfolio(Guid portfolioId)
        {
            logger.LogInformation("Getting all portfolioItems for portfolio with id: {@portfolioId}", portfolioId);
            await GetAuhtorizedAsync(portfolioId);
            var portfolios = await portfolioItemRepository.GetAllPortfolioItemInPortfolio(portfolioId);

            return mapper.Map<IEnumerable<PortfolioItemDto>>(portfolios);

        }

        public async Task<PortfolioItemDto> GetByIdAsync(Guid portfolioId, Guid portfolioItemId)
        {
            logger.LogInformation("Getting portfolioItem with id: {@id}", portfolioItemId);
            await GetAuhtorizedAsync(portfolioId);
            var portfolioItem = await portfolioItemRepository.GetPortfolioItemByIdAsync(portfolioItemId)
                ?? throw new NotImplementedException("Portfolio item not found");

            var dto = mapper.Map<PortfolioItemDto>(portfolioItem);
            await EnrichItemWithMarketDataAsync(dto);
            return dto;
        }

        public async Task UpdateAsync(UpdatePortfolioItemDto dto, Guid portfolioItem, Guid portfolioItemId)
        {
            logger.LogInformation("Updating portfolioItem");
            await GetAuhtorizedAsync(portfolioItem);

            var oldPortfolio = await portfolioRepository.GetByIdAsync(portfolioItemId)
                ?? throw new NotImplementedException("Not found");

            mapper.Map(dto, oldPortfolio);
            await portfolioItemRepository.UpdateAsync();

        }
    }
}
