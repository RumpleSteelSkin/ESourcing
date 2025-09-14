using ESourcing.Sourcing.Entities;

namespace ESourcing.Sourcing.Repositories.Interfaces;

public interface IAuctionRepository
{
    Task<IEnumerable<Auction>> GetAllAsync();
    Task<Auction?> GetByIdAsync(string id);
    Task<Auction> GetByNameAsync(string name);
    
    Task CreateAsync(Auction auction);
    Task<bool> UpdateAsync(Auction auction);
    Task<bool> DeleteAsync(string id);
}