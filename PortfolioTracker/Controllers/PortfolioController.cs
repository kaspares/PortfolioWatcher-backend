using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;

namespace PortfolioTracker.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/portfolios")]
    public class PortfolioController(IPortfolioService portfolioService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortfolioSummaryDto>>> GetAllByUser()
        {
            var result = await portfolioService.GetUserPortfoliosAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PortfolioDetailDto>> GetByIdWithItems(Guid id)
        {
            var result = await portfolioService.GetByIdWithItemsAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePortfolioDto portfolio)
        {
            await portfolioService.CreateAsync(portfolio);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdatePortfolioDto portfolio, Guid id)
        {
            await portfolioService.UpdateAsync(portfolio, id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await portfolioService.DeleteAsync(id);

            return NoContent();
        }
    }
}
