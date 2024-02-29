using Api.Integrations.Interfaces;
using RabbitMQ.Client;

namespace Api.Integrations
{
    public class RabbitMqSetupService : IServiceBusSetup
    {
        private readonly IModel _channel;

        public RabbitMqSetupService(IModel channel)
        {
            _channel = channel;
        }

        public void Setup()
        {
            // Crear el exchange "AGP" si no existe
            _channel.ExchangeDeclare(exchange: "AGP", type: ExchangeType.Topic, durable: true);

            // Crear colas duraderas y nombradas para cada tópico
            var queueName = "eventsQueue";
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: queueName, exchange: "AGP", routingKey: "event.*");
        }
    }
}
