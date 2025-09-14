using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Data.Seeds;
using ESourcing.Sourcing.Entities;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data.Concretes;

public class SourcingContext : ISourcingContext
{
    public SourcingContext(IConfiguration configuration)
    {
        var database = new MongoClient(configuration["SourcingDatabaseSettings:ConnectionString"]).GetDatabase(configuration["SourcingDatabaseSettings:DatabaseName"]);
        Auctions = database.GetCollection<Auction>(configuration["SourcingDatabaseSettings:CollectionAuctionName"]);
        Bids = database.GetCollection<Bid>(configuration["SourcingDatabaseSettings:CollectionBidName"]);
        SourcingContextSeed.SeedData(Auctions);
    }

    public IMongoCollection<Auction> Auctions { get; }
    public IMongoCollection<Bid> Bids { get; }
}