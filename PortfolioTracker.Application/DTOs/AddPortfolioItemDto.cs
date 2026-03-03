namespace PortfolioTracker.Application.DTOs;

public class AddPortfolioItemDto
{
    public string Ticker { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
}
