namespace CarRental.Automotor.Infrastructure.Event
{
    public class RabbitMqSettings
    {
        public const string SectionName = "RabbitMq";

        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string PaymentQueueName { get; set; } = "payment.processed";
    }
}
