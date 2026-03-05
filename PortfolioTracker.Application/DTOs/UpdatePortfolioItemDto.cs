namespace PortfolioTracker.Application.DTOs;

public class UpdatePortfolioItemDto
{
    public string Ticker { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public List<ItemCommentDto> Comments { get; set; } = [];
}
