using ESourcing.Core.Entities;
using ESourcing.Core.Repositories;
using ESourcing.Core.Repositories.Base;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repository;
using ESourcing.Infrastructure.Repository.Base;
using ESourcing.UI.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WebAppContext>(opt =>
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]));
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 4;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireDigit = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireLowercase = false;
        opt.User.AllowedUserNameCharacters = "qwertyuıopğüasdfghjklşi,<zxcvbnmöç. QWERTYUIOPĞÜASDFGHJKLŞİZXCVBNMÖÇ";
    }).AddDefaultTokenProviders()
    .AddEntityFrameworkStores<WebAppContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Home/Login";
    options.LogoutPath = $"/Home/Logout";
});

builder.Services.AddSession(opt => { opt.IdleTimeout = TimeSpan.FromMinutes(20); });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddHttpClient();

builder.Services.AddHttpClient<ProductClient>();
builder.Services.AddHttpClient<AuctionClient>();
builder.Services.AddHttpClient<BidClient>();


builder.Services.AddAuthentication().AddGoogle(opt =>
{
    opt.ClientSecret = builder.Configuration["ExternalOAuths:Google:Secret"] ?? string.Empty;
    opt.ClientId = builder.Configuration["ExternalOAuths:Google:ID"] ?? string.Empty;
});




var app = builder.Build();

#region Auto Migration And Seed

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WebAppContext>();
    await context.Database.MigrateAsync();
    await WebAppContextSeed.SeedAsync(context, scope.ServiceProvider.GetRequiredService<ILoggerFactory>());
}

#endregion

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();