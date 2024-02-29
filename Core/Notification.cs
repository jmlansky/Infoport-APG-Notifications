using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Status { get; set; } = string.Empty;
        public IEnumerable<NotificationDetail> Detalles { get; set; } = new List<NotificationDetail>();
    }
}
