using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface INotificationStrategy
    {
        public string Strategy { get; }
        void Send(Notification notification);
    }
}
