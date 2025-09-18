using AutoMapper;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events.Concretes;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Sourcing.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuctionController(
    IAuctionRepository auctionRepository,
    IBidRepository bidRepository,
    IMapper mapper,
    EventBusRabbitMQProducer eventBus,
    ILogger<AuctionController> logger) : ControllerBase
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


    [HttpPost("CompleteAuction/{id:length(24)}", Name = "CompleteAuction")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult> CompleteAuction(string id)
    {
        var auction = await auctionRepository.GetByIdAsync(id);
        if (auction is null) return NotFound();
        if (auction.Status != (int)Status.Active)
        {
            logger.LogError("Auction cannot be completed");
            return BadRequest();
        }

        var bid = await bidRepository.GetWinnerBidAsync(id);
        if (bid is null) return NotFound();

        var eventMessage = mapper.Map<OrderCreateEvent>(bid);
        eventMessage.Quantity = auction.Quantity;

        auction.Status = (int)Status.Closed;
        var updateResponse = await auctionRepository.UpdateAsync(auction);
        if (!updateResponse)
        {
            logger.LogError("Failed to update auction");
            return BadRequest();
        }

        try
        {
            eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
        }
        catch (Exception e)
        {
            logger.LogError(e, "ERROR Publishing integration event: {EventMessageId} from Sourcing", eventMessage.Id);
            throw;
        }

        return Accepted();
    }


    [HttpPost("TestEvent")]
    [ProducesResponseType(typeof(OrderCreateEvent),StatusCodes.Status202Accepted)]
    public ActionResult TestEvent()
    {
        var eventMessage = new OrderCreateEvent
        {
            AuctionId = "dummy1",
            ProductId = "dummy_product_1",
            Price = 10,
            Quantity = 100,
            SellerUserName = "test@test.com"
        };
        
        try
        {
            eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
        }
        catch (Exception e)
        {
            logger.LogError(e, "ERROR Publishing integration event: {EventMessageId} from Sourcing", eventMessage.Id);
            throw;
        }

        return Accepted(eventMessage);
    }
}