using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Sourcing.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuctionController(IAuctionRepository auctionRepository, ILogger<AuctionController> logger) : ControllerBase
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<Auction>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Auction>>> GetAll()
    {
        return Ok(await auctionRepository.GetAllAsync());
    }

    [HttpGet("GetById/{id:length(24)}", Name = "GetById")]
    [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Auction>> GetById(string id)
    {
        var auction = await auctionRepository.GetByIdAsync(id);
        if (auction is not null) return Ok(auction);
        logger.LogError("No auction found with id {Id}", id);
        return NotFound();
    }

    [HttpPost("Create")]
    [ProducesResponseType(typeof(Auction), StatusCodes.Status201Created)]
    public async Task<ActionResult<Auction>> Create([FromBody] Auction auction)
    {
        await auctionRepository.CreateAsync(auction);
        return CreatedAtRoute("GetById", new { id = auction.Id }, auction);
    }

    [HttpPut("Update")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> Update([FromBody] Auction auction)
    {
        return Ok(await auctionRepository.UpdateAsync(auction));
    }

    [HttpDelete("Delete/{id:length(24)}", Name = "Delete")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> Delete(string id)
    {
        return Ok(await auctionRepository.DeleteAsync(id));
    }
}