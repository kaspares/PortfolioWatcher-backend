namespace PortfolioTracker.Domain.ValueObjects;

public sealed record PriceCandle(
    DateTime Data,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    long Volume
    );
