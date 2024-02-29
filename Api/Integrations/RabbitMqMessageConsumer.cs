using Api.Integrations.Interfaces;
using Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.interfaces;
using System.Text;

namespace Api.Integrations
{
    public class RabbitMqMessageConsumer(IServiceProvider serviceProvider, IModel channel) : IMessageConsumer
    {
        private const int MaxRetryAttempts = 5;
        private const string RetryCountHeader = "x-retry-count";
        private readonly IModel channel = channel;
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private const string STATUS_PENDING = "PENDING";

        public void StartConsuming(string queueName)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                using (var scope = serviceProvider.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var notificationService = services.GetRequiredService<INotificationService>();
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;

                    try
                    {
                        if (!await ProcessMessage(notificationService, message, routingKey, ea))                        
                            HandleFailedMessage(ea, retryCount);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.Message}");
                        HandleFailedMessage(ea, retryCount);
                    }
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private async Task<bool> ProcessMessage(INotificationService notificationService, string message, string routingKey, BasicDeliverEventArgs ea)
        {
            var notification = new Notification
            {
                FechaHora = DateTime.Now,
                Type = routingKey,
                Status = STATUS_PENDING,
                Detalles = new List<NotificationDetail> { new() { Body = message } }
            };

            var result = await notificationService.ProcessNotificationAsync(notification);
            if (result)
            {
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                return true;
            }
            return false;
        }


        private void HandleFailedMessage(BasicDeliverEventArgs ea, int retryCount)
        {
            if (retryCount >= MaxRetryAttempts)
            {
                // TODO Considerar mover el mensaje a una cola de "muertos" o registrar el fallo permanentemente
                Console.WriteLine("Message processing failed after max attempts, discarding or logging.");
                channel.BasicAck(ea.DeliveryTag, false);
            }
            else
            {
                var properties = channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> { { RetryCountHeader, retryCount + 1 } };
                channel.BasicPublish(exchange: "", routingKey: ea.RoutingKey, basicProperties: properties, body: ea.Body);
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        private static int GetRetryCount(IBasicProperties properties)
        {
            if (properties.Headers != null && properties.Headers.TryGetValue(RetryCountHeader, out var value) && value is byte[] bytes)            
                return BitConverter.ToInt32(bytes, 0);            

            return 0;
        }
    }

}
