using ESourcing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESourcing.Infrastructure.Data;

public class WebAppContextSeed
{
    public static async Task SeedAsync(WebAppContext context, ILoggerFactory loggerFactory, int retryCount = 0)
    {
        const int maxRetries = 50;
        var logger = loggerFactory.CreateLogger<WebAppContextSeed>();
        for (var attempt = retryCount; attempt < maxRetries; attempt++)
        {
            try
            {
                await context.Database.MigrateAsync();
                if (!context.AppUsers.Any())
                {
                    await context.AppUsers.AddRangeAsync(GetPreconfiguredUsers());
                    await context.SaveChangesAsync();
                }
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                if (attempt >= maxRetries - 1)
                    throw;
                await Task.Delay(2000);
            }
        }
    }

    private static List<AppUser> GetPreconfiguredUsers() =>
    [
        new AppUser
        {
            FirstName = "User1",
            LastName = "User LastName1",
            IsSeller = true,
            IsBuyer = false
        }
    ];
}