using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Evento
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Datos { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }
}
