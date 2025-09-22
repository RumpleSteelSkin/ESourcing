using ESourcing.Order.Domain.Repositories.Base;

namespace ESourcing.Order.Domain.Repositories;

public interface IOrderRepository : IRepository<Entities.Order>
{
    Task<IEnumerable<Entities.Order>> GetOrdersBySellerUserName(string userName);
}