using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core
{
    public class Agente : IAgente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public ICollection<AgenteSuscripcion> AgenteSuscripciones { get; set; } = new List<AgenteSuscripcion>();
    }
}
