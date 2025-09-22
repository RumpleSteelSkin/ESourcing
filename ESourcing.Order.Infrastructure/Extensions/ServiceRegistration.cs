using System.Runtime.CompilerServices;
using ESourcing.Order.Domain.Repositories;
using ESourcing.Order.Domain.Repositories.Base;
using ESourcing.Order.Infrastructure.Data;
using ESourcing.Order.Infrastructure.Repositories;
using ESourcing.Order.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESourcing.Order.Infrastructure.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
            options.UseSqlServer(configuration["ConnectionStrings:OrderConnection"],
                b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)));
        
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IOrderRepository, OrderRepository>();
        return services;
    }
}