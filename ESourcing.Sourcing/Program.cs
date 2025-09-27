using System.Reflection;
using ESourcing.Sourcing.Data.Concretes;
using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Hubs;
using ESourcing.Sourcing.Repositories.Concretes;
using ESourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddTransient<ISourcingContext, SourcingContext>();
builder.Services.AddTransient<IAuctionRepository, AuctionRepository>();
builder.Services.AddTransient<IBidRepository, BidRepository>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", corsPolicyBuilder =>
{
    corsPolicyBuilder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

#region EventBus

builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory
    {
        HostName = builder.Configuration["EventBus:HostName"],
        UserName = builder.Configuration["EventBus:UserName"],
        Password = builder.Configuration["EventBus:Password"],
        Port = Convert.ToInt32(builder.Configuration["EventBus:Port"]),
    };
    var retryCount = Convert.ToInt32(builder.Configuration["EventBus:RetryCount"]);

    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
});

builder.Services.AddSingleton<EventBusRabbitMQProducer>();

#endregion

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapHub<AuctionHub>("/auctionhub");

app.UseHttpsRedirection();
app.MapControllers();


app.Run();