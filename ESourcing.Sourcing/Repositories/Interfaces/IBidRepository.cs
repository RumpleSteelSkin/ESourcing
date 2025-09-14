using ESourcing.Sourcing.Entities;

namespace ESourcing.Sourcing.Repositories.Interfaces;

public interface IBidRepository
{
    Task SendBidAsync(Bid bid);
    Task<IEnumerable<Bid>> GetByAuctionIdAsync(string id);
    Task<IEnumerable<Bid>> GetAllByAuctionIdAsync(string id);
    Task<Bid> GetWinnerBidAsync(string id);
}