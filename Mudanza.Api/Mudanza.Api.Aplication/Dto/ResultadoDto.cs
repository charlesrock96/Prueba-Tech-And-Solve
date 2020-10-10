using System;
using System.Collections.Generic;
using System.Text;

namespace Mudanza.Api.Aplication.Dto
{
    public class ResultadoDto
    {
        public bool Error { get; set; }
        public string ErrorMensaje { get; set; }
        public string Salida { get; set; }
    }
}
