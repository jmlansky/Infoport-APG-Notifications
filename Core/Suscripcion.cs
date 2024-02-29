using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Suscripcion
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public ICollection<AgenteSuscripcion> AgenteSuscripciones { get; set; } = new List<AgenteSuscripcion>();
    }
}
