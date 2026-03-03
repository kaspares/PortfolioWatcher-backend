using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Application.DTOs
{
    public class PortfolioDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PortfolioItemDto> Items { get; set; } = [];
    }
}
