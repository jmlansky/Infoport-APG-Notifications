using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class AgenteSuscripcion
    {
        public int AgenteId { get; set; }
        public Agente Agente { get; set; } = new();

        public int SuscripcionId { get; set; }
        public Suscripcion Suscripcion { get; set; } = new();
    }
}
