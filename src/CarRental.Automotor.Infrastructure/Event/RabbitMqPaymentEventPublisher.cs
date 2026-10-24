using CarRental.Automotor.Application.Events;
using CarRental.Automotor.Application.IService;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace CarRental.Automotor.Infrastructure.Event
{
    public class RabbitMqPaymentEventPublisher : IPaymentEventPublisher
    {
        private readonly RabbitMqSettings _settings;

        public RabbitMqPaymentEventPublisher(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
        }

        public async Task PublishPaymentProcessedAsync(PaymentProcessedEvent paymentEvent)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _settings.PaymentQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(paymentEvent);

            var properties = new BasicProperties
            {
                ContentType = "application/json",
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _settings.PaymentQueueName,
                mandatory: true,
                basicProperties: properties,
                body: body);
        }
    }
}
