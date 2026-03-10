using PortfolioTracker.Domain.ValueObjects;

namespace PortfolioTracker.Domain.Interfaces;

public interface IMarketDataProvider
{
    Task<IReadOnlyList<PriceCandle>> GetHistoricalPricesAsync(string ticker, int periods = 200);
    Task<decimal> GetCurrentPriceAsync(string ticker);
}
