using System.Text;
using System.Text.Json;
using Clinic.Application.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Clinic.Infrastructure.Messaging
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IChannel _channel;

        public NotificationConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _connection = await factory.CreateConnectionAsync(stoppingToken);
                    _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                    await _channel.QueueDeclareAsync(
                        queue: "notifications_queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: stoppingToken);

                    var consumer = new AsyncEventingBasicConsumer(_channel);
                    
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var message = JsonSerializer.Deserialize<AppointmentCreatedMessage>(json);

                        Console.WriteLine($"[Consumer] Congratulations! I have a new appointment: {message?.Title}");
                        
                        await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    };

                    await _channel.BasicConsumeAsync("notifications_queue", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                    Console.WriteLine("[Consumer] Connected to RabbitMQ successfully and listening for messages!");
                    break; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Consumer] Connection to RabbitMQ failed: {ex.Message}. Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null && _channel.IsOpen)
            {
                await _channel.CloseAsync(cancellationToken: cancellationToken);
                _channel.Dispose();
            }
            
            if (_connection != null && _connection.IsOpen)
            {
                await _connection.CloseAsync(cancellationToken: cancellationToken);
                _connection.Dispose();
            }
            
            await base.StopAsync(cancellationToken);
        }
    }
}