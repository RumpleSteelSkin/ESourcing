using ESourcing.Order.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOrderServices(builder.Configuration);

var app = builder.Build();

await app.AddOrderApplications();
