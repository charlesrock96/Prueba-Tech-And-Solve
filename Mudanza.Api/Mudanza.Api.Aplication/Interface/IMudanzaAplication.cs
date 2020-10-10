using Microsoft.AspNetCore.Http;
using Mudanza.Api.Aplication.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mudanza.Api.Aplication.Interface
{
    public interface IMudanzaAplication
    {
        public Task<ResultadoDto> EjecutarProcesoMudanzaAsync(MudanzaDto mudanza);
        public string ProcesarDatos(EntradasDto estrutura);
        public Task<string[]> CargarArchivoAsync(IFormFile archivo);
        public EntradasDto LeerArregloDatos(String[] datos);
        public Task<ResultadoDto> ValidarDatosAsync(MudanzaDto mudanza);
        public Task<string> ValidarDatosArchivoAsync(IFormFile archivo);
    }
}
