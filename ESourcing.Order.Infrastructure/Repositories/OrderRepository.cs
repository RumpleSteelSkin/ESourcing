using ESourcing.Order.Domain.Repositories;
using ESourcing.Order.Infrastructure.Data;
using ESourcing.Order.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ESourcing.Order.Infrastructure.Repositories;

public class OrderRepository(OrderContext dbContext) : Repository<Domain.Entities.Order>(dbContext), IOrderRepository
{
    private readonly OrderContext _dbContext = dbContext;

    public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersBySellerUserName(string userName)
    {
        return await _dbContext.Orders.Where(o => o.SellerUserName == userName).ToListAsync();
    }
}