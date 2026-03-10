namespace PortfolioTracker.Application.DTOs
{
    public class PortfolioItemDto
    {
        public Guid Id { get; set; }
        public string Ticker { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<ItemCommentDto> Comments { get; set; } = [];
        public decimal? CurrentPrice { get; set; }
        public decimal? MarketValue { get; set; }
        public decimal? ProfitLoss { get; set; }
        public decimal? ProfitLossPercent { get; set; }
    }
}