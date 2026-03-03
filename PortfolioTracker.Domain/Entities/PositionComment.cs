namespace PortfolioTracker.Domain.Entities;

public class PositionComment : BaseEntity
{
    public Guid PortfolioItemId { get; set; }
    public string Content { get; set; }
}