namespace ESourcing.Order.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
        }
    }

    private static List<Domain.Entities.Order> GetPreconfiguredOrders()
    {
        return
        [
            new Domain.Entities.Order
            {
                AuctionId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                SellerUserName = "test@test.com",
                UnitPrice = 10,
                TotalPrice = 1000,
                CreatedAt = DateTime.UtcNow
            },

            new Domain.Entities.Order
            {
                AuctionId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                SellerUserName = "test1@test.com",
                UnitPrice = 10,
                TotalPrice = 1000,
                CreatedAt = DateTime.UtcNow
            },

            new Domain.Entities.Order
            {
                AuctionId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                SellerUserName = "test2@test.com",
                UnitPrice = 10,
                TotalPrice = 1000,
                CreatedAt = DateTime.UtcNow
            }
        ];
    }
}