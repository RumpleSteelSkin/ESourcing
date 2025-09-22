using ESourcing.Order.Consumers;
using ESourcing.Order.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ESourcing.Order.Extensions;

public static class AppRegistration
{
    private static EventBusOrderCreateConsumer Listener { get; set; } = null!;

    public static async Task AddOrderApplications(this WebApplication app)
    {
        #region Swagger

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        #endregion

        #region Middlewares

        app.UseHttpsRedirection();
        app.MapControllers();

        #endregion

        #region Auto Migration And Seed

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
            await context.Database.MigrateAsync();
            await OrderContextSeed.SeedAsync(context);
        }

        #endregion

        #region RabbitMQ Consume

        Listener = app.Services.GetRequiredService<EventBusOrderCreateConsumer>();
        var life = app.Services.GetRequiredService<IHostApplicationLifetime>();
        life.ApplicationStarted.Register(() => Listener.Consume());
        life.ApplicationStopping.Register(() => Listener.Disconnect());

        #endregion

        await app.RunAsync();
    }
}