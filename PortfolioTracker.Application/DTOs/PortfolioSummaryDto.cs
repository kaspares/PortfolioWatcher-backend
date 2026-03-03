namespace PortfolioTracker.Application.DTOs;

public class PortfolioSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ItemCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
