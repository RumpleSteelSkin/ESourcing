using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories.Concretes;

public class AuctionRepository(ISourcingContext context) : IAuctionRepository
{
    public async Task<IEnumerable<Auction>> GetAllAsync()
    {
        return await context.Auctions.Find(p => true).ToListAsync();
    }

    public async Task<Auction?> GetByIdAsync(string id)
    {
        return await context.Auctions.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Auction> GetByNameAsync(string name)
    {
        return await context.Auctions.Find(p => p.Name == name).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Auction auction)
    {
        await context.Auctions.InsertOneAsync(auction);
    }

    public async Task<bool> UpdateAsync(Auction auction)
    {
        var updateResult = await context.Auctions.ReplaceOneAsync(p => p.Id == auction.Id, auction);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleteResult = await context.Auctions.DeleteOneAsync(p => p.Id == id);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}