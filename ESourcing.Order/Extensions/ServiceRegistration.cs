using System.Reflection;
using ESourcing.Order.Application.Extensions;
using ESourcing.Order.Consumers;
using ESourcing.Order.Infrastructure.Extensions;
using EventBusRabbitMQ;
using RabbitMQ.Client;

namespace ESourcing.Order.Extensions;

public static class ServiceRegistration
{
    public static void AddOrderServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Core Services

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        #endregion

        #region Application & Infrastructure

        services.AddApplication();
        services.AddInfrastructure(configuration);

        #endregion

        #region EventBus / RabbitMQ

        services.AddSingleton<IRabbitMQPersistentConnection>(sp => new DefaultRabbitMQPersistentConnection(
            new ConnectionFactory
            {
                HostName = configuration["EventBus:HostName"],
                UserName = configuration["EventBus:UserName"],
                Password = configuration["EventBus:Password"],
                Port = Convert.ToInt32(configuration["EventBus:Port"]),
            }, Convert.ToInt32(configuration["EventBus:RetryCount"]),
            sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>()));
        services.AddSingleton<EventBusOrderCreateConsumer>();
        #endregion
    }
}