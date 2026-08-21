using System.Text;
using System.Text.Json;
using Clinic.Notification.Api.Data;
using Clinic.Notification.Api.Entities;
using Clinic.Notification.Api.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Clinic.Notification.Api.Messaging
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
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
                    var connection = await factory.CreateConnectionAsync(stoppingToken);
                    var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                    await channel.QueueDeclareAsync(
                        queue: "notifications_queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: stoppingToken);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);

                        // 1. قراءة الـ JSON وتحويله مباشرة للـ DTO
                        var message = JsonSerializer.Deserialize<AppointmentCreatedMessage>(json);

                        if (message != null)
                        {
                            // 2. حفظ الإشعار بالداتابيز بالـ Entity الجديدة والمرنة
                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

                                var notification = new NotificationLog
                                {
                                    UserId = message.PatientId,
                                    Title = "تأكيد حجز ميعاد",
                                    Message = $"تم حجز الميعاد ({message.Title}) بتاريخ {message.AppointmentDate:yyyy-MM-dd HH:mm}",
                                    Type = "Appointment",
                                    IsRead = false,
                                    CreatedAt = DateTime.UtcNow
                                };

                                dbContext.Notifications.Add(notification);
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }

                            Console.WriteLine($"[Notification Microservice] Saved notification for appointment: {message.Title}");
                        }

                        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    };

                    await channel.BasicConsumeAsync("notifications_queue", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
                    Console.WriteLine("[Notification Microservice] Connected & Listening...");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Consumer Error] Retrying in 5s... {ex.Message}");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}