using Microsoft.Extensions.Logging;
using PortfolioTracker.Domain.Interfaces;
using PortfolioTracker.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PortfolioTracker.Infrastructure.MarketData
{
    public class YahooFinanceProvider(ILogger<YahooFinanceProvider> logger,
        HttpClient httpClient) : IMarketDataProvider
    {
        public async Task<decimal> GetCurrentPriceAsync(string ticker)
        {
            var candles = await GetHistoricalPricesAsync(ticker, 1);
            return candles.LastOrDefault()?.Close
                ?? throw new InvalidOperationException($"No price data available for {ticker}.");
        }

        public async Task<IReadOnlyList<PriceCandle>> GetHistoricalPricesAsync(string ticker, int periods = 200)
        {
            try
            {
                var endDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var startDate = DateTimeOffset.UtcNow.AddDays(-periods * 2).ToUnixTimeSeconds(); // Extra days for weekends/holidays

                var url = $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}?period1={startDate}&period2={endDate}&interval=1d";
                
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var document = JsonDocument.Parse(json);

                var result = document.RootElement
                    .GetProperty("chart")
                    .GetProperty("result")[0];

                var timestamps = result.GetProperty("timestamp");
                var quote = result.GetProperty("indicators").GetProperty("quote")[0];

                var opens = quote.GetProperty("open");
                var highs = quote.GetProperty("high");
                var lows = quote.GetProperty("low");
                var closes = quote.GetProperty("close");
                var volumes = quote.GetProperty("volume");

                var candles = new List<PriceCandle>();
                for (int i = 0; i < timestamps.GetArrayLength(); i++)
                {
                    if (closes[i].ValueKind == JsonValueKind.Null) continue;

                    candles.Add(new PriceCandle(
                        DateTimeOffset.FromUnixTimeSeconds(timestamps[i].GetInt64()).UtcDateTime,
                        opens[i].ValueKind != JsonValueKind.Null ? opens[i].GetDecimal() : 0,
                        highs[i].ValueKind != JsonValueKind.Null ? highs[i].GetDecimal() : 0,
                        lows[i].ValueKind != JsonValueKind.Null ? lows[i].GetDecimal() : 0,
                        closes[i].GetDecimal(),
                        volumes[i].ValueKind != JsonValueKind.Null ? volumes[i].GetInt64() : 0
                    ));
                }
                return candles.TakeLast(periods).ToList();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch historical prices for {Ticker}", ticker);
                throw;
            }
        }
    }
}
