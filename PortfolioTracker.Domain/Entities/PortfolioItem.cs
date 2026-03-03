namespace PortfolioTracker.Domain.Entities;

public class PortfolioItem : BaseEntity
{
    public Guid PortfolioId { get; set; }
    public string Ticker { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime PurchaseDate { get; set; }
    public List<PositionComment> Comments { get; set; } = [];

}