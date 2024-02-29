using Api.Requests;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService notificationService = notificationService;

        [HttpGet("config")]
        public async Task<IActionResult> GetConfigs()
        {
            var result = await notificationService.GetNotificationConfigs();
            return Ok(result);
        }

        [HttpPost("notifications-susbcribe")]
        public async Task<IActionResult> NotificationsSubscribe([FromBody] NotificationsSubscribeRequest request)
        {
            if (!request.Suscripciones.Any())
                return BadRequest("Seleccione alguna suscripcion: PUSH, EMAIL, IMPRESION");


            var agente = new Agente
            {
                Nombre = request.Nombre,
                Tipo = request.TipoAgente,                
                AgenteSuscripciones = new List<AgenteSuscripcion>()
            };

            foreach (var tipoSuscripcion in request.Suscripciones)
            {
                var suscripcion = new Suscripcion { Tipo = tipoSuscripcion };
                var agenteSuscripcion = new AgenteSuscripcion
                {
                    Agente = agente,
                    Suscripcion = suscripcion
                };
                
                agente.AgenteSuscripciones.Add(agenteSuscripcion);
            }

            await notificationService.NotificationsSubscribeAsync(agente);

            return Ok();
        }

    }
}
