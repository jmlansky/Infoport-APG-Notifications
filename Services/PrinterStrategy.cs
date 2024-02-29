using Core;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PrinterStrategy : INotificationStrategy
    {
        public string Strategy { get => "PRINTER"; }

        public void Send(Notification notification)
        {
            // Send list to print
            notification.Status = "SENT PRINTER";
        }
    }
}
