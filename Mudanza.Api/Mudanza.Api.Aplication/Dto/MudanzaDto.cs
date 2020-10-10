using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mudanza.Api.Aplication.Dto
{
    public class MudanzaDto
    {
        public string Cedula { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
