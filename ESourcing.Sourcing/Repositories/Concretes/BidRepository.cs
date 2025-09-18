using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories.Concretes;

public class BidRepository(ISourcingContext context) : IBidRepository
{
    public async Task SendBidAsync(Bid bid)
    {
        await context.Bids.InsertOneAsync(bid);
    }

    public async Task<IEnumerable<Bid>> GetByAuctionIdAsync(string id)
    {
        var bids = await context.Bids.Find(a => a.AuctionId == id)
            .SortByDescending(a => a.CreatedAt)
            .ToListAsync();

        return bids
            .GroupBy(b => b.SellerUserName)
            .Select(g => g.First())
            .ToList();
    }

    public async Task<IEnumerable<Bid>> GetAllByAuctionIdAsync(string id)
    {
        return await context.Bids.Find(p => p.AuctionId == id)
            .SortByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Bid?> GetWinnerBidAsync(string id)
    {
        var bids = await GetAllByAuctionIdAsync(id);
        return bids.OrderByDescending(a => a.Price).FirstOrDefault()
               ?? throw new InvalidOperationException($"No bids found for auction with id: {id}");
    }
}
