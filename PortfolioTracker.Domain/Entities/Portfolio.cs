namespace PortfolioTracker.Domain.Entities;

public class Portfolio : BaseEntity
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PortfolioItem> Items { get; set; } = [];
}
