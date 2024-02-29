using Core;
using Repositories.Interfaces;
using Services.interfaces;

namespace Services
{
    public class NotificationService(INotificationsRepository notificationsRepository, IEnumerable<INotificationStrategy> notificationStrategies) : INotificationService
    {
        private readonly INotificationsRepository notificationsRepository = notificationsRepository;
        private readonly IEnumerable<INotificationStrategy> notificationStrategies = notificationStrategies;

        public async Task<Agente?> GetByAgenteId(int id)
        {
            try
            {
                var agentes = await notificationsRepository.GetAsync<Agente>();
                return agentes.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<NotificationConfig>> GetNotificationConfigs()
        {
            return await notificationsRepository.GetAsync<NotificationConfig>();
        }

        public async Task<bool> NotificationsSubscribeAsync(Agente agente)
        {
            try
            {
                var result = await SaveAsync(agente);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ProcessNotificationAsync(Notification notification)
        {
            try
            {
                var result = await SaveAsync(notification);
                if (!result)
                    return false;

                result = await SendAsync(notification);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> SaveAsync<T>(T item) where T : class
        {
            try
            {
                await notificationsRepository.SaveAsync(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> SendAsync(Notification notification)
        {
            try
            {
                var subscriptions = await notificationsRepository.GetAsync<Suscripcion>();
                var tasks = notificationStrategies.Select(strategy =>
                {
                    // Obtener todos los agentes suscriptos a cada estrategia
                    var sub = subscriptions.FirstOrDefault(x => x.Tipo == strategy.Strategy);
                    if (sub != null)
                    {
                        var agentesSuscriptos = sub.AgenteSuscripciones.Select(a => a.Agente).ToList();
                        strategy.Send(notification);
                    }

                    return Task.CompletedTask;

                }).ToList();

                await Task.WhenAll(tasks);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
