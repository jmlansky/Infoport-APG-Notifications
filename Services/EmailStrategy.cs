using Core;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmailStrategy : INotificationStrategy
    {
        public string Strategy { get => "EMAIL"; }

        public void Send(Notification notification)
        {
            // Send email
            notification.Status = "SENT EMAIL";
        }
    }
}
