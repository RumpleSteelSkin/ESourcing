using ESourcing.Products.Data.Interfaces;
using ESourcing.Products.Data.Seeds;
using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Concretes;

public class ProductContext : IProductContext
{
    public ProductContext(IConfiguration configuration)
    {
        Products = new MongoClient(configuration["ProductDatabaseSettings:ConnectionString"])
            .GetDatabase(configuration["ProductDatabaseSettings:DatabaseName"])
            .GetCollection<Product>(configuration["ProductDatabaseSettings:CollectionName"]);
        ProductContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}