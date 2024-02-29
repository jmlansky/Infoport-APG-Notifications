using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface INotificationService
    {
        Task<Agente?> GetByAgenteId(int id);
        Task<IEnumerable<NotificationConfig>> GetNotificationConfigs();
        Task<bool> NotificationsSubscribeAsync(Agente agente);
        Task<bool> ProcessNotificationAsync(Notification notification);
        
    }
}
