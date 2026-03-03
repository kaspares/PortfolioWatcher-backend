using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Domain.Repositories;

namespace PortfolioTracker.Application.Services;

public class PortfolioService(ILogger<PortfolioService> logger,
    IMapper mapper,
    IPortfolioRepository portfolioRepository,
    ICurrentUserService currentUser) : IPortfolioService
{
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
        var portfolioDto = await GetByIdWithItemsAsync(id);
        
        var portfolio = mapper.Map<Portfolio>(portfolioDto);

        await portfolioRepository.DeleteAsync(portfolio);
    }

    public async Task<PortfolioDetailDto> GetByIdWithItemsAsync(Guid id)
    {
        logger.LogInformation("Getting portfolio with id: {@Id}", id);
        var portfolio = await portfolioRepository.GetByIdWithItemsAsync(id)
            ?? throw new NotImplementedException("Not found");
        if (portfolio.UserId != currentUser.userId)
            throw new NotImplementedException("Forbidden");

        return mapper.Map<PortfolioDetailDto>(portfolio);

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


        mapper.Map(portfolioDto, portfolio);
        await portfolioRepository.UpdateAsync(portfolio);
    }
}
