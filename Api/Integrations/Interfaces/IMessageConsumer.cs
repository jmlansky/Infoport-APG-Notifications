namespace Api.Integrations.Interfaces
{
    public interface IMessageConsumer
    {
        void StartConsuming(string queueName);
    }
}
