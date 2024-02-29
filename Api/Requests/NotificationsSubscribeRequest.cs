using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Api.Requests
{
    public class NotificationsSubscribeRequest
    {
        [Required(ErrorMessage ="Por favor ingrese un Id de agente.")]
        [Min(1, ErrorMessage ="El id de agente tiene que ser un numero positivo")]
        public int IdAgente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor ingrese un Tipo de agente.")]
        public string TipoAgente{ get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor ingrese un Tipo de agente.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public IEnumerable<string> Suscripciones { get; set; } = new List<string>();
    }
}
