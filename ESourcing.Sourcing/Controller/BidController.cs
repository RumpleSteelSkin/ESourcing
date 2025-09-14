using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Sourcing.Controller;

[Route("api/[controller]")]
[ApiController]
public class BidController(IBidRepository bidRepository, ILogger<BidController> logger) : ControllerBase
{
    [HttpPost("SendBid")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendBid(Bid bid)
    {
        await bidRepository.SendBidAsync(bid);
        return Ok("Bid sent");
    }

    [HttpGet("GetBidsByAuctionId/{id:length(24)}", Name = "GetBidsByAuctionId")]
    [ProducesResponseType(typeof(IEnumerable<Bid>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string id)
    {
        return Ok(await bidRepository.GetAllByAuctionIdAsync(id));
    }

    [HttpGet("GetWinnerBid/{id:length(24)}", Name = "GetWinnerBid")]
    [ProducesResponseType(typeof(Bid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Bid>> GetWinnerBid(string id)
    {
        return Ok(await bidRepository.GetWinnerBidAsync(id));
    }
}