using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Application.Services;

namespace PortfolioTracker.API.Controllers
{
    [ApiController]
    [Route("/api/{id}/portfolioItems")]
    public class PortfolioItemController(IPortfolioItemService portfolioItemService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortfolioItemDto>>> GetAllPortfolioItemsByPortfolio(Guid id)
        {
            var result = await portfolioItemService.GetAllByPortfolio(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("{portfolioItemId}")]
        public async Task<ActionResult<PortfolioItemDto>> GetPortfolioItemById(Guid portfolioItemId)
        {
            var result = await portfolioItemService.GetByIdAsync(portfolioItemId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePortfolioItemDto portfolioItemDto, Guid id)
        {
            await portfolioItemService.CreateAsync(portfolioItemDto, id);
            return NoContent();
        }

        [HttpPut("{portfolioItemId}")]
        public async Task<IActionResult> Update(UpdatePortfolioItemDto portfolioItemDto, Guid portfolioItemId)
        {
            await portfolioItemService.UpdateAsync(portfolioItemDto, portfolioItemId);

            return NoContent();
        }

        [HttpDelete("{portfolioItemId}")]
        public async Task<IActionResult> Delete(Guid portfolioItemId)
        {
            await portfolioItemService.DeleteAsync(portfolioItemId);

            return NoContent();
        }

    }
}
