using Microsoft.EntityFrameworkCore;

namespace ESourcing.Order.Infrastructure.Data;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.Order> Orders { get; set; }
}