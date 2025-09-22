using System.Reflection;
using ESourcing.Order.Application.Pipelines;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ESourcing.Order.Application.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            opt.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            opt.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            opt.AddOpenBehavior(typeof(TransactionalBehaviour<,>));
            opt.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
        });
        return services;
    }
}