using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ;

public class DefaultRabbitMQPersistentConnection(
    IConnectionFactory connectionFactory,
    int retryCount,
    ILogger<DefaultRabbitMQPersistentConnection> logger)
    : IRabbitMQPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory =
        connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

    private readonly int _retryCount = retryCount > 0 ? retryCount : 5;

    private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private IConnection? _connection;
    private bool _disposed;
    private readonly object _lock = new();

    public bool IsConnected => _connection is { IsOpen: true } && !_disposed;

    public bool TryConnect()
    {
        _logger.LogInformation("Trying to connect to RabbitMQ...");

        lock (_lock)
        {
            var policy = Policy
                .Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        _logger.LogError(ex,
                            "RabbitMQ connection attempt failed. Retrying in {Delay}s. Exception: {Message}",
                            time.TotalSeconds, ex.Message);
                    });

            policy.Execute(() =>
            {
                _connection?.Dispose();
                _connection = _connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                _connection!.ConnectionShutdown += OnConnectionShutDown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation(
                    "RabbitMQ persistent connection established to host {HostName}",
                    _connection.Endpoint.HostName);

                return true;
            }

            _logger.LogCritical("FATAL ERROR: RabbitMQ connection could not be established.");
            return false;
        }
    }

    private void OnConnectionShutDown(object? sender, ShutdownEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning("RabbitMQ connection shutdown: {ReplyText}", e.ReplyText);
        TryConnect();
    }

    private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning(e.Exception, "RabbitMQ callback exception occurred.");
        TryConnect();
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning("RabbitMQ connection blocked. Reason: {Reason}", e.Reason);
        TryConnect();
    }

    public IModel CreateModel()
    {
        return !IsConnected
            ? throw new InvalidOperationException("RabbitMQ connection is not open.")
            : _connection!.CreateModel();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        try
        {
            _connection?.Dispose();
        }
        catch (IOException ex)
        {
            _logger.LogCritical(ex, "Error disposing RabbitMQ connection.");
        }

        GC.SuppressFinalize(this);
    }
}