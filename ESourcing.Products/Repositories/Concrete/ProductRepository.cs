using ESourcing.Products.Data.Interfaces;
using ESourcing.Products.Entities;
using ESourcing.Products.Repositories.Interfaces;
using MongoDB.Driver;

namespace ESourcing.Products.Repositories.Concrete;

public class ProductRepository(IProductContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await context.Products.Find(p => true).ToListAsync();
    }

    public async Task<Product?> GetAsync(string id)
    {
        return await context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(string name)
    {
        return await context.Products.Find(p => p.Name == name).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryNameAsync(string categoryName)
    {
        return await context.Products.Find(p=>p.Category == categoryName).ToListAsync();
    }

    public async Task Create(Product product)
    {
        await context.Products.InsertOneAsync(product);
    }

    public async Task<bool> Update(Product product)
    {
        await context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        var result = await context.Products.DeleteOneAsync(p=>p.Id == id);
        return result.DeletedCount > 0;
    }
}