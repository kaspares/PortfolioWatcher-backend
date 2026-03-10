using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Domain.Interfaces;
using PortfolioTracker.Domain.Repositories;

namespace PortfolioTracker.Application.Services;

public class PortfolioService(ILogger<PortfolioService> logger,
    IMapper mapper,
    IPortfolioRepository portfolioRepository,
    ICurrentUserService currentUser,
    IMarketDataProvider marketDataProvider) : IPortfolioService
{
    private async Task EnrichWithMarketDataAsync(PortfolioDetailDto dto)
    {
        var tickers = dto.Items.Select(i => i.Ticker).Distinct();
        var prices = new Dictionary<string, decimal>();

        foreach (var ticker in tickers)
        {
            try
            {
                prices[ticker] = await marketDataProvider.GetCurrentPriceAsync(ticker);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to fetch price for {Ticker}", ticker);
            }
        }

        foreach (var item in dto.Items)
        {
            if (!prices.TryGetValue(item.Ticker, out var price)) continue;

            item.CurrentPrice = price;
            item.MarketValue = item.Quantity * price;
            var cost = item.Quantity * item.PurchasePrice;
            item.ProfitLoss = item.MarketValue - cost;
            item.ProfitLossPercent = cost != 0 ? Math.Round(item.ProfitLoss.Value / cost * 100, 2) : 0;
        }

        dto.TotalValue = dto.Items.Sum(i => i.MarketValue ?? 0);
        dto.TotalCost = dto.Items.Sum(i => i.Quantity * i.PurchasePrice);
        dto.TotalProfitLoss = dto.TotalValue - dto.TotalCost;
        dto.TotalProfitLossPercent = dto.TotalCost != 0
            ? Math.Round(dto.TotalProfitLoss.Value / dto.TotalCost.Value * 100, 2) : 0;
    }

    public async Task CreateAsync(CreatePortfolioDto dto)
    {
        logger.LogInformation("Creating new portfolio: {@Portfolio}", dto);

        var portfolio = mapper.Map<Portfolio>(dto);
        portfolio.UserId = currentUser.userId;
        
        await portfolioRepository.AddAsync(portfolio);
    }

    public async Task DeleteAsync(Guid id)
    {
        logger.LogInformation("Deleting portfolio with id: {@Id}", id);
        var portfolio = await portfolioRepository.GetByIdAsync(id)
            ?? throw new Exception("Portfolio not found");

        await portfolioRepository.DeleteAsync(portfolio);
    }

    public async Task<PortfolioDetailDto> GetByIdWithItemsAsync(Guid id)
    {
        logger.LogInformation("Getting portfolio with id: {@Id}", id);
        var portfolio = await portfolioRepository.GetByIdWithItemsAsync(id)
            ?? throw new NotImplementedException("Not found");
        if (portfolio.UserId != currentUser.userId)
            throw new NotImplementedException("Forbidden");

        var dto = mapper.Map<PortfolioDetailDto>(portfolio);
        await EnrichWithMarketDataAsync(dto);
        return dto;
    }

    public async Task<IEnumerable<PortfolioSummaryDto>> GetUserPortfoliosAsync()
    {
        logger.LogInformation("Getting portfolios for {@CurrentUser}", currentUser.userId);
        var userPortfolios = await portfolioRepository.GetAllByUserIdAsync(currentUser.userId)
            ?? throw new NotImplementedException("Not found");

        return mapper.Map<IEnumerable<PortfolioSummaryDto>>(userPortfolios);
    }

    public async Task UpdateAsync(UpdatePortfolioDto dto, Guid id)
    {
        logger.LogInformation("Deleting portfolio with id: {@Id}", id);
        var portfolioDto = await GetByIdWithItemsAsync(id);

        var portfolio = mapper.Map<Portfolio>(portfolioDto);

        mapper.Map(dto, portfolio);
        await portfolioRepository.UpdateAsync(portfolio);
    }
}
