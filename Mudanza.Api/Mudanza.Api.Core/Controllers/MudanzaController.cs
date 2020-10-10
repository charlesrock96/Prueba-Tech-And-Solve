using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mudanza.Api.Aplication.Dto;
using Mudanza.Api.Aplication.Interface;

namespace Mudanza.Api.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MudanzaController : ControllerBase
    {
        private readonly IMudanzaAplication _mudanzaAplication;

        public MudanzaController(IMudanzaAplication mudanzaAplication)
        {
            _mudanzaAplication = mudanzaAplication;
        }

        // POST: api/Medanza
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] MudanzaDto mudanza)
        {
            ResultadoDto resultado = await _mudanzaAplication.EjecutarProcesoMudanzaAsync(mudanza);
            
            return Ok(resultado);
        }
    }
}
