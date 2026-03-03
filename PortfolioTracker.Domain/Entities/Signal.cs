using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Domain.Entities;

public class Signal : BaseEntity
{
    public string UserId { get; set; }
    public string Ticker { get; set; }
    public IndicatorType IndicatorType { get; set; }
    public SignalType SignalType { get; set; }
    public decimal CurrentValue { get; set; }
    public decimal ThresholdValue { get; set; }
}
