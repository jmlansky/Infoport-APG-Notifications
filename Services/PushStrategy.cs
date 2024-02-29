using Core;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PushStrategy : INotificationStrategy
    {
        public string Strategy { get => "PUSH"; }

        public void Send(Notification notification)
        {
            //SEND push notification
            notification.Status = "SENT PUSH";
        }
    }
}
