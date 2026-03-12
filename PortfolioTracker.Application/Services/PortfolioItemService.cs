using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Exceptions;
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

        private async Task GetAuthorizedAsync(Guid portfolioId)
        {
            var portfolio = await portfolioRepository.GetByIdAsync(portfolioId)
              ?? throw new NotImplementedException("Portfolio not found");

            if (portfolio.UserId != currentUser.userId)
                throw new NotImplementedException("Forbidden");
        }
        public async Task CreateAsync(CreatePortfolioItemDto dto, Guid portfolioId)
        {
            logger.LogInformation("Creating new portfolio item {@Dto} in portfolio with id: {@PortfolioId}", dto, portfolioId);
            
            await GetAuthorizedAsync(portfolioId);

            var portfolioItem = mapper.Map<PortfolioItem>(dto);
            portfolioItem.PortfolioId = portfolioId;

            await portfolioItemRepository.AddAsync(portfolioItem);

        }

        public async Task DeleteAsync(Guid portfolioId, Guid portfolioItemId)
        {
            logger.LogInformation("Deleting portfolioItem with id: {@PortfolioItemId}", portfolioItemId);
            await GetAuthorizedAsync(portfolioId);

            var portfolioItem = await portfolioItemRepository.GetPortfolioItemByIdAsync(portfolioItemId) 
                ?? throw new NotFoundException(nameof(PortfolioItem), portfolioItemId.ToString());

            await portfolioItemRepository.DeleteAsync(portfolioItem);
        }

        public async Task<IEnumerable<PortfolioItemDto>> GetAllByPortfolio(Guid portfolioId)
        {
            logger.LogInformation("Getting all portfolioItems for portfolio with id: {@PortfolioId}", portfolioId);
            await GetAuthorizedAsync(portfolioId);
            var portfolios = await portfolioItemRepository.GetAllPortfolioItemInPortfolio(portfolioId);

            return mapper.Map<IEnumerable<PortfolioItemDto>>(portfolios);

        }

        public async Task<PortfolioItemDto> GetByIdAsync(Guid portfolioId, Guid portfolioItemId)
        {
            logger.LogInformation("Getting portfolioItem with id: {@PortfolioItemId}", portfolioItemId);
            await GetAuthorizedAsync(portfolioId);
            var portfolioItem = await portfolioItemRepository.GetPortfolioItemByIdAsync(portfolioItemId)
                ?? throw new NotFoundException(nameof(PortfolioItem), portfolioItemId.ToString());

            var dto = mapper.Map<PortfolioItemDto>(portfolioItem);
            await EnrichItemWithMarketDataAsync(dto);
            return dto;
        }

        public async Task UpdateAsync(UpdatePortfolioItemDto dto, Guid portfolioItem, Guid portfolioItemId)
        {
            logger.LogInformation("Updating portfolioItem with id: {@PortfolioItemId}", portfolioItemId);
            await GetAuthorizedAsync(portfolioItem);

            var oldPortfolio = await portfolioItemRepository.GetPortfolioItemByIdAsync(portfolioItemId)
                ?? throw new NotFoundException(nameof(PortfolioItem), portfolioItemId.ToString());

            mapper.Map(dto, oldPortfolio);
            await portfolioItemRepository.UpdateAsync();

        }
    }
}
